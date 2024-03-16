using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
using wg.sharedForTests.Factories.Owners;
using wg.sharedForTests.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class GroupControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddUserToGroup_GivenAuthorizedManagerAndExistingFields_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        var group = GroupFactory.GetGroupInOwner(owner);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.Manager());
        var command = new AddUserToGroupCommand(Guid.Empty, user.Id);

        //act
        var response = await HttpClient.PostAsJsonAsync($"/owner-module/groups/{group.Id.Value}/add-user", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        var changedGroup = await _ownerDbContext
            .Groups
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == group.Id);
        changedGroup!.Users.Any().ShouldBeTrue();
    }
    
    [Fact]
    public async Task AddUserToGroup_GivenAuthorizedManagerAndOneNotExistingField_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var group = GroupFactory.GetGroupInOwner(owner);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.Manager());
        var command = new AddUserToGroupCommand(Guid.Empty, Guid.NewGuid());

        //act
        var response = await HttpClient.PostAsJsonAsync($"/owner-module/groups/{group.Id.Value}/add-user", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var changedGroup = await _ownerDbContext
            .Groups
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == group.Id);
        changedGroup!.Users.Any().ShouldBeFalse();
    }
    
    [Fact]
    public async Task AddUserToGroup_GivenAuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        var command = new AddUserToGroupCommand(Guid.Empty, Guid.NewGuid());

        //act
        var response = await HttpClient.PostAsJsonAsync($"/owner-module/groups/{Guid.NewGuid()}/add-user", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task AddUserToGroup_GivenUnauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddUserToGroupCommand(Guid.Empty, Guid.NewGuid());

        //act
        var response = await HttpClient.PostAsJsonAsync($"/owner-module/groups/{Guid.NewGuid()}/add-user", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    #region arrange
    private readonly TestDb _testDb;
    private readonly OwnerDbContext _ownerDbContext;

    public GroupControllerTests()
    {
        _testDb = new TestDb();
        _ownerDbContext = _testDb.OwnerDbContext;
    }

    public override void Dispose()
    {
        _testDb.Dispose();
    }

    #endregion
}