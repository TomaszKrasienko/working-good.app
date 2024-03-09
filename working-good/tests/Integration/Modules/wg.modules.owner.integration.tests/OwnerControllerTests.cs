using System.Net;
using System.Net.Http.Json;
using Shouldly;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.integration.tests._Helpers;
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
        response.Headers.TryGetValues("resource-id", out var values);
        values!.Single().ShouldNotBe(Guid.Empty.ToString());
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    #region arrange
    private readonly OwnerDbContext _ownerDbContext;

    public OwnerControllerTests()
    {
        _ownerDbContext = new TestDb().OwnerDbContext;
    }
    #endregion
}