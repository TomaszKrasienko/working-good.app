using System.Net;
using System.Net.Http.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.SignIn;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.integration.tests._Helpers;
using wg.shared.abstractions.Auth.DTOs;
using wg.shared.infrastructure.Exceptions.DTOs;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class UsersControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetAll_GivenPaginationArgumentsForExistingUser_ShouldReturn200OkStatusCodeWithUserDto()
    {
        //arrange
        await AddOwner(true, true);
        Authorize(Guid.NewGuid(), Role.User());
        var query = new GetUsersQuery()
        {
            PageNumber = 1,
            PageSize = 10
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetUsersQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetUsersQuery.PageNumber), query.PageNumber.ToString());
        
        //act
        var response = await HttpClient.GetAsync($"owner-module/users?{queryString.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        result.ShouldNotBeEmpty();
        
        var pagination = GetPaginationMetaDataFromHeader(response);
        pagination.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetAll_GivenPaginationArgumentsForNotExistingUsers_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        await AddOwner(false, false);
        Authorize(Guid.NewGuid(), Role.User());
        var query = new GetUsersQuery()
        {
            PageNumber = 1,
            PageSize = 10
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetUsersQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetUsersQuery.PageNumber), query.PageNumber.ToString());
        
        //act
        var response = await HttpClient.GetAsync($"owner-module/users?{queryString.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        var pagination = GetPaginationMetaDataFromHeader(response);
        pagination.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetAll_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var query = new GetUsersQuery()
        {
            PageNumber = 1,
            PageSize = 10
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetUsersQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetUsersQuery.PageNumber), query.PageNumber.ToString());
        
        //act
        var response = await HttpClient.GetAsync($"owner-module/users?{queryString.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetForGroup_GivenExistingGroupId_ShouldReturnUsersInGroup()
    {
        //arrange
        var owner = await AddOwner(false, false);
        var group = await AddGroup(owner);
        var userInGroup = await AddUser(owner);
        var userNotInGroup = await AddUser(owner);
        owner.AddUserToGroup(group.Id, userInGroup.Id);
        OwnerDbContext.Owner.Update(owner);
        await OwnerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //arrange
        var response = await HttpClient.GetFromJsonAsync<List<UserDto>>($"owner-module/users/group/{group.Id.Value}");
        
        //assert
        response.ShouldBeOfType<List<UserDto>>();
        response?.Any(x => x.Id.Equals(userInGroup.Id)).ShouldBeTrue();
        response?.Any(x => x.Id.Equals(userNotInGroup.Id)).ShouldBeFalse();
    }
    
    [Fact]
    public async Task GetForGroup_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var response = await HttpClient.GetAsync($"owner-module/users/group/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetActiveUserById_GivenExistingActiveUserId_ShouldReturnUserDto()
    {
        //arrange
        await AddOwner(true, true);
        var user = await GetUser();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<UserDto>($"owner-module/users/{user.Id.Value}/active");
        
        //assert
        result.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetActiveUserById_GivenNotExistingActiveUserId_ShouldReturnUserDto()
    {
        //arrange
        await AddOwner(true, false);
        var user = await GetUser();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"owner-module/users/{user.Id.Value}/active");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetActiveUserById_Unauthorized_ShouldReturnUserDto()
    {
        //arrange
        await AddOwner(true, true);
        var user = await GetUser();
        
        //act
        var response = await HttpClient.GetAsync($"owner-module/users/{user.Id.Value}/active");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Me_GivenAuthorizedUser_ShouldReturnUserDto()
    {
        //arrange
        await AddOwner(true, true);
        var user = await GetUser();
        Authorize(user.Id, user.Role);
        
        //act
        var result = await HttpClient.GetFromJsonAsync<UserDto>("owner-module/users/me");
        
        //assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Me_GivenUnauthorizedUser_ShouldReturnUnauthorized()
    {
        //act
        var result = await HttpClient.GetAsync("owner-module/users/me");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task SignUp_GivenSignUpCommandWithExistingOwner_ShouldReturn200StatusCodeAndSaveUserInDb()
    {
        //arrange
        await AddOwner(false, false);
        var command = new SignUpCommand(Guid.Empty, "joe.doe@test.pl", "Joe", "Doe",
            "MyPass123!", Role.Manager());

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-up", command);

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var user = await GetUserByEmail(command.Email);
        user.ShouldNotBeNull();
    }

    [Fact]
    public async Task SignUp_GivenSignUpCommandWithoutExistingOwner_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new SignUpCommand(Guid.Empty, "joe.doe@test.pl", "Joe", "Doe",
            "MyPass123!", Role.Manager());

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-up", command);

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Verify_GivenExistingVerificationToken_ShouldReturn200OkStatusCodeAndUpdateUserInDb()
    {
        //arrange
        var owner = await AddOwner(true, false);
        var user = await GetUser();
        var command = new VerifyUserCommand(user!.VerificationToken.Token);

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/verify", command);

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var updatedUser = await GetUserByEmail(user.Email);
        updatedUser!.State.ShouldBe(State.Activate());
    }

    [Fact]
    public async Task Verify_GivenNotExistingToken_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AddOwner(true, false);
        var command = new VerifyUserCommand("invalid_token");

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/verify", command);

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignIn_GivenSignUpCommandWithValidCredentials_ShouldReturn200OkStatusCodeWithJwtDto()
    {
        //arrange
        await AddOwner(true, true);
        var user = await GetUser();
        var command = new SignInCommand(user!.Email, user!.Password);

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-in", command);

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var token = await result.Content.ReadFromJsonAsync<JwtDto>();
        token?.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task SignIn_GivenSignUpCommandWithInvalidCredentials_ShouldReturn400BadRequestStatusCodeWithWrongCredentialsMessage()
    {
        //arrange
        await AddOwner(true, true);
        var user = await GetUser();
        var command = new SignInCommand(user!.Email, "invalid_password");

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-in", command);

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var response = await result.Content.ReadFromJsonAsync<ErrorDto>();
        response!.Message.ShouldBe("Wrong credentials");
    }

    private async Task<Owner> AddOwner(bool withUser, bool withVerifiedUser)
    {
        var owner = OwnerFactory.Get();
        if (withUser)
        {
            var user = UserFactory.GetUserInOwner(owner, Role.Manager());
            if (withVerifiedUser)
            {
                user.Verify(DateTime.Now);
            }
        }
        await OwnerDbContext.Owner.AddAsync(owner);
        await OwnerDbContext.SaveChangesAsync();
        return owner;
    }

    private async Task<User> AddUser(Owner owner)
    {
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        OwnerDbContext.Owner.Update(owner);
        await OwnerDbContext.SaveChangesAsync();
        return user;
    }

    private async Task<Group> AddGroup(Owner owner)
    {
        var group = GroupFactory.GetGroupInOwner(owner);
        OwnerDbContext.Owner.Update(owner);
        await OwnerDbContext.SaveChangesAsync();
        return group;
    }
    
    private Task<User> GetUser()
        => OwnerDbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync();

    private Task<User> GetUserByEmail(string email)
        => OwnerDbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);

    #region arrange
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordManager, TestPasswordManager>();
    }
    #endregion
}