using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Companies.Commands.AddCompany;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.integration.tests._Helpers;
using wg.modules.owner.domain.ValueObjects.User;
using wg.sharedForTests.Factories.Companies;
using wg.sharedForTests.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

[Collection("#1")]
public sealed class CompaniesControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetById_GivenExistingIdAndAuthorized_ShouldReturnCompanyDto()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetEmployeeInCompany(company);
        var project = ProjectFactory.GetInCompany(company, true, true);
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<CompanyDto>($"companies-module/Companies/{company.Id.Value}");
        
        //assert
        result.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetById_GivenNotExistingIdAndAuthorized_ShouldReturnNoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetAsync($"companies-module/Companies/{Guid.NewGuid().ToString()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetById_GivenNotAuthorized_ShouldReturnUnauthorizedStatusCode()
    {
        //act
        var result = await HttpClient.GetAsync($"companies-module/Companies/{Guid.NewGuid().ToString()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddCompany_GivenAddCompanyCommandWithAuthorizedManager_ShouldReturnStatusCodeOk()
    {
        //arrange
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
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == command.Name);
        company.ShouldNotBeNull();
    }

    [Fact]
    public async Task AddCompany_NotGivenAuthorizeHeader_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.Empty, "NewCompanyTest", TimeSpan.FromHours(10), "test.pl");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddCompany_GivenAuthorizeForUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.Empty, "NewCompanyTest", TimeSpan.FromHours(10), "test.pl");
        Authorize(Guid.NewGuid(), Role.User());
        //act
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task AddCompany_GivenInvalidAddCompanyCommand_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.Empty, string.Empty, TimeSpan.FromHours(10), "test.pl");
        Authorize(Guid.NewGuid(), Role.Manager());
        //act
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
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