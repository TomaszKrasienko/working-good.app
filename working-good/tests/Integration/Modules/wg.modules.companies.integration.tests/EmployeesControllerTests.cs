using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

[Collection("#1")]
public sealed class EmployeesControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddEmployee_GivenExistingCompanyIdAndAddEmployeeCommandAndAuthorizedManager_ShouldReturn201CreatedStatusCodeWithResourceIdHeaderAndLocationHeaderAndAddedEmployeeToDb()
    {
        //arrange
        var company = await AddCompany();
        var command = new AddEmployeeCommand(Guid.Empty, Guid.Empty, $"joe.doe@{company.EmailDomain.Value}", "555-555-555");
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/employees/companies/{company.Id.Value}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var employee = await GetEmployeeByIdAsync((Guid)resourceId);
        employee.ShouldNotBeNull();
        employee.Email.Value.ShouldBe(command.Email);
    }
    
    [Fact]
    public async Task AddEmployee_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddEmployeeCommand(Guid.Empty, Guid.Empty, $"joe.doe@test.pl", "555-555-555");
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/employees/companies/{Guid.NewGuid()}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddEmployee_AuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var command = new AddEmployeeCommand(Guid.Empty, Guid.Empty, $"joe.doe@test.pl", "555-555-555");
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/employees/companies/{Guid.NewGuid()}/add", command);
        
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

    private async Task<Company> AddCompany()
    {
        var company = CompanyFactory.Get();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        return company;
    }
    
    private Task<Employee> GetEmployeeByIdAsync(Guid id)
        => _companiesDbContext
        .Employees
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id.Equals(id))!;
    
    #region arrange
    private readonly TestAppDb _testAppDb;
    private readonly CompaniesDbContext _companiesDbContext;

    public EmployeesControllerTests()
    {
        _testAppDb = new TestAppDb();
        _companiesDbContext = _testAppDb.CompaniesDbContext;
    }

    public override void Dispose()
    {
        _testAppDb.Dispose();
    }
    #endregion
}