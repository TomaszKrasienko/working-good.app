using System.Net;
using System.Net.Http.Json;
using Shouldly;
using wg.modules.companies.application.CQRS.Companies.AddCompany;
using wg.modules.owner.domain.ValueObjects.User;
using wg.sharedForTests.Factories.Owner;
using wg.sharedForTests.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

public sealed class CompaniesControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddCompany_GivenAddCompanyCommandWithAuthorizedManager_ShouldReturnStatusCodeOk()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        var command = new AddCompanyCommand(Guid.Empty, "NewCompanyTest", TimeSpan.FromHours(10), "test.pl");
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var result = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    
}