using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.DTOs;
using wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class GroupControllerTests : BaseTestsController
{
    [Fact]
    public async Task IsMembershipExists_GivenExistingMembership_ShouldReturnIsExistsDtoWithTrueValue()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        user.Verify(DateTime.Now);
        group.AddUser(user);

        await OwnerDbContext.Owner.AddAsync(owner);
        await OwnerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"owner-module/groups/{group.Id.Value}/{user.Id.Value}/is-membership-exists");
        
        //assert
        result.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task IsMembershipExists_GivenNotExistingMembership_ShouldReturnIsExistsDtoWithFalseValue()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        user.Verify(DateTime.Now);

        await OwnerDbContext.Owner.AddAsync(owner);
        await OwnerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"owner-module/groups/{group.Id.Value}/{user.Id.Value}/is-membership-exists");
        
        //assert
        result.Value.ShouldBeFalse();
    }

    [Fact]
    public async Task IsMembershipExists_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"owner-module/groups/{Guid.NewGuid()}/{Guid.NewGuid()}/is-membership-exists");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddUserToGroup_GivenAuthorizedManagerAndExistingGroupIdAndUserId_ShouldReturn204NoContentStatusCodeAndAddUserToGroup()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        await OwnerDbContext.Owner.AddAsync(owner);
        await OwnerDbContext.SaveChangesAsync();
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
        var group = GroupFactory.GetInOwner(owner);
        await OwnerDbContext.Owner.AddAsync(owner);
        await OwnerDbContext.SaveChangesAsync();
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

    private Task<Group> GetGroupByIdAsync(Guid id)
        =>  OwnerDbContext
            .Groups
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
}