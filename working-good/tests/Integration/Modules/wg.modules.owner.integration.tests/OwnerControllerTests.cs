using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
using wg.sharedForTests.Factories.Owner;
using wg.sharedForTests.Integration;
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
    
    #region arrange
    private readonly TestDb _testDb;
    private readonly OwnerDbContext _ownerDbContext;

    public OwnerControllerTests()
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