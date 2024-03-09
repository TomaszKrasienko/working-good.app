using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
using wg.sharedForTests.Factories.Owner;
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

    #endregion
}