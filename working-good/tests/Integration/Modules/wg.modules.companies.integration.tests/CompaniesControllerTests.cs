using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Companies.AddCompany;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.integration.tests._Helpers;
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
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.TryGetValues("resource-id", out var values);
        values!.Single().ShouldNotBe(Guid.Empty.ToString());
        var company = await _companiesDbContext
            .Companies
            .FirstOrDefaultAsync(x => x.Name == command.Name);
        company.ShouldNotBeNull();
    }

    #region arrange

    private readonly TestDb _testDb;
    private readonly CompaniesDbContext _companiesDbContext;

    public CompaniesControllerTests()
    {
        _testDb = new TestDb();
        _companiesDbContext = _testDb.CompaniesDbContext;
    }

    public override void Dispose()
    {
        _testDb.Dispose();
    }

    #endregion
}