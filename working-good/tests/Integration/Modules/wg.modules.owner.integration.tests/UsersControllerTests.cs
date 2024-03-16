using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.SignIn;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
using wg.shared.abstractions.Auth.DTOs;
using wg.shared.infrastructure.Exceptions.DTOs;
using wg.sharedForTests.Factories.Owners;
using wg.sharedForTests.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class UsersControllerTests : BaseTestsController
{
    [Fact]
    public async Task SignUp_GivenSignUpCommandForExistingOwner_ShouldReturn200StatusCodeAndSaveUserInDb()
    {
        //arrange
        var owner = OwnerFactory.Get();
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        var command = new SignUpCommand(Guid.Empty, "joe.doe@test.pl", "Joe", "Doe",
            "MyPass123!", Role.Manager());

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-up", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var user = await _ownerDbContext.Users.FirstOrDefaultAsync();
        user.ShouldNotBeNull();
    }

    [Fact]
    public async Task SignUp_GivenSignUpCommandWithoutExistingOwner_ShouldReturn400BadRequestStatusCodeAndNotSaveUserInDb()
    {
        //arrange
        var command = new SignUpCommand(Guid.Empty, "joe.doe@test.pl", "Joe", "Doe",
            "MyPass123!", Role.Manager());

        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-up", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var user = await _ownerDbContext.Users.FirstOrDefaultAsync();
        user.ShouldBeNull();
    }
    
    [Fact]
    public async Task Verify_GivenExistingToken_ShouldReturn200OkStatusCodeAndUpdateUserInDb()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        var command = new VerifyUserCommand(user.VerificationToken.Token);
        
        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/verify", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var verifiedUser = await _ownerDbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == user.Id);
        verifiedUser!.State.ShouldBe(State.Activate());
    }
    
    [Fact]
    public async Task Verify_GivenNotExistingToken_ShouldReturnBadRequestStatusCode()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        var command = new VerifyUserCommand("invalid_token");
        
        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/verify", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignIn_GivenSignUpCommandWithValidCredentials_ShouldReturn200OkStatusCodeWithToken()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        owner.VerifyUser(user.VerificationToken.Token, DateTime.Now);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        var command = new SignInCommand(user.Email, user.Password);
        
        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-in", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var token = await result.Content.ReadFromJsonAsync<JwtDto>();
        token.ShouldNotBeNull();
        token.Token.ShouldNotBeNullOrWhiteSpace(); 
    }
    
    [Fact]
    public async Task SignIn_GivenSignUpCommandWithInvalidCredentials_ShouldReturnBadRequestStatusCodeWithWrongCredentialsMessage()
    {
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        owner.VerifyUser(user.VerificationToken.Token, DateTime.Now);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        var command = new SignInCommand(user.Email, "invalid_password");
        
        //act
        var result = await HttpClient.PostAsJsonAsync("/owner-module/users/sign-in", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var response = await result.Content.ReadFromJsonAsync<ErrorDto>();
        response!.Message.ShouldBe("Wrong credentials");
    }
    
    #region arrange

    private readonly TestDb _testDb;
    private readonly OwnerDbContext _ownerDbContext;
    
    public UsersControllerTests()
    {
        _testDb = new TestDb();
        _ownerDbContext = _testDb.OwnerDbContext;
    }

    public override void Dispose()
    {
        _testDb.Dispose();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordManager, TestPasswordManager>();
    }

    #endregion
}