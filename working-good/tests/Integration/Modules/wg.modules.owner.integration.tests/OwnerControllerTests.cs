using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.application.CQRS.Owners.Commands.ChangeOwnerName;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class OwnerControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddOwner_GivenAddOwnerCommand_ShouldReturnCreatedStatusCodeWithResourceHeader()
    {
        //arrange
        var command = new AddOwnerCommand(Guid.Empty, "owner_company_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/owner-module/owner/add", command);
     
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        response.Headers.TryGetValues("resource-id", out var values);
        values!.Single().ShouldNotBe(Guid.Empty.ToString());
        var owner = await _ownerDbContext.Owner.FirstOrDefaultAsync();
        owner.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddOwner_GivenAddOwnerCommandForExistingOwner_ShouldReturnBadRequestStatusCode()
    {
        //arrange
        await _ownerDbContext.Owner.AddAsync(OwnerFactory.Get());
        await _ownerDbContext.SaveChangesAsync();
        var command = new AddOwnerCommand(Guid.Empty, "owner_company_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/owner-module/owner/add", command);
     
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChangeOwnerName_GivenNewNameAndAuthorizedManager_ShouldReturn204NoContentStatusCodeAndChangeNameInDb()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(user.Id, user.Role);
        var companyNewName = "MyCompanyNewName";
        var command = new ChangeOwnerNameCommand(companyNewName);
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        var changedOwner = await _ownerDbContext.Owner
            .AsNoTracking()
            .FirstOrDefaultAsync();
        changedOwner!.Name.Value.ShouldBe(companyNewName);
    }
    
    [Fact]
    public async Task ChangeOwnerName_GivenNewNameAndWithoutAuthorizedUser_ShouldReturn401Unauthorized()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        var companyNewName = "MyCompanyNewName";
        var command = new ChangeOwnerNameCommand(companyNewName);
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task ChangeOwnerName_GivenNewNameAndAuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var manager = UserFactory.GetUserInOwner(owner, Role.Manager());
        var user = UserFactory.GetUserInOwner(owner, Role.User());
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(user.Id, user.Role);
        var companyNewName = "MyCompanyNewName";
        var command = new ChangeOwnerNameCommand(companyNewName);
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task ChangeOwnerName_GivenEmptyNameAndAuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(user.Id, user.Role);
        var command = new ChangeOwnerNameCommand(string.Empty);
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    #region arrange
    private readonly TestAppDb _testDb;
    private readonly OwnerDbContext _ownerDbContext;

    public OwnerControllerTests()
    {
        _testDb = new TestAppDb();
        _ownerDbContext = _testDb.OwnerDbContext;
    }

    public override void Dispose()
    {
        _testDb.Dispose();
    }

    #endregion
}