using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.integration.tests._Helpers;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

[Collection("#1")]
public sealed class EmployeesControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddEmployee_GivenExistingCompanyIdAndValidAddEmployeeCommandAndAuthorizeHeader_ShouldReturn201CreatedStatusCodeWithHeaders()
    {
        //arrange
        var company = CompanyFactory.Get();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        var command = new AddEmployeeCommand(Guid.Empty, Guid.Empty, $"joe.doe@{company.EmailDomain.Value}", "555-555-555");
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/employees/companies/{company.Id.Value}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.TryGetValues("resource-id", out var values);
        values!.Single().ShouldNotBe(Guid.Empty.ToString());
        var employee = await _companiesDbContext
            .Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == command.Email);
        employee.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddEmployee_NotGivenAuthorizeHeader_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var company = CompanyFactory.Get();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        var command = new AddEmployeeCommand(Guid.Empty, Guid.Empty, $"joe.doe@{company.EmailDomain.Value}", "555-555-555");
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/employees/companies/{company.Id.Value}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddEmployee_GivenAuthorizeHeaderAsUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var company = CompanyFactory.Get();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        var command = new AddEmployeeCommand(Guid.Empty, Guid.Empty, $"joe.doe@{company.EmailDomain.Value}", "555-555-555");
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/employees/companies/{company.Id.Value}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task AddEmployee_GivenNotExistingCompanyId_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new AddEmployeeCommand(Guid.Empty, Guid.Empty, $"joe.doe@test.pl", "555-555-555");
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/employees/companies/{Guid.NewGuid()}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    #region arrange
    private readonly TestDb _testDb;
    private readonly CompaniesDbContext _companiesDbContext;

    public EmployeesControllerTests()
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