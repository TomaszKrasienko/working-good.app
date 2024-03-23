using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class GroupControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddUserToGroup_GivenAuthorizedManagerAndExistingGroupIdAndUserId_ShouldReturn204NoContentStatusCodeAndAddUserToGroup()
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
        
        var updatedGroup = await GetGroupByIdAsync(group.Id);
        updatedGroup!.Users.Any(x => x.Id == user.Id).ShouldBeTrue();
    }
    
    [Fact]
    public async Task AddUserToGroup_GivenAuthorizedManagerAndNotExistingUser_ShouldReturn400BadRequestStatusCode()
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
    public async Task AddUserToGroup_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddUserToGroupCommand(Guid.Empty, Guid.NewGuid());

        //act
        var response = await HttpClient.PostAsJsonAsync($"/owner-module/groups/{Guid.NewGuid()}/add-user", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    private Task<Group?> GetGroupByIdAsync(Guid id)
        =>  _ownerDbContext
            .Groups
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    
    #region arrange
    private readonly TestAppDb _testAppDb;
    private readonly OwnerDbContext _ownerDbContext;

    public GroupControllerTests()
    {
        _testAppDb = new TestAppDb();
        _ownerDbContext = _testAppDb.OwnerDbContext;
    }

    public override void Dispose()
    {
        _testAppDb.Dispose();
    }
    #endregion
}