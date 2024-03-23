using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.SignIn;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
using wg.shared.abstractions.Auth.DTOs;
using wg.shared.infrastructure.Exceptions.DTOs;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class UsersControllerTests : BaseTestsController
{
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
        var command = new SignInCommand(user.Email, "invalid_password");

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
                user.VerificationToken.Verify(DateTime.Now);
            }
        }
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        return owner;
    }
    
    private Task<User?> GetUser()
        => _ownerDbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync();

    private Task<User?> GetUserByEmail(string email)
        => _ownerDbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);

    #region arrange
    private readonly TestAppDb _testAppDb;
    private readonly OwnerDbContext _ownerDbContext;
    
    public UsersControllerTests()
    {
        _testAppDb = new TestAppDb();
        _ownerDbContext = _testAppDb.OwnerDbContext;
    }

    public override void Dispose()
    {
        _testAppDb.Dispose();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordManager, TestPasswordManager>();
    }
    #endregion
}