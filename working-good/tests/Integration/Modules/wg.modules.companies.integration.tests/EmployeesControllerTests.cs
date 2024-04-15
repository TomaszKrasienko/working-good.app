using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;
using wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.domain.ValueObjects.User;
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
        var company = await AddCompanyAsync();
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

    [Fact]
    public async Task DeactivateEmployee_GivenExistingEmployeeIdAndSubstituteEmployeeId_ShouldReturn200OkStatusCode()
    {
        //arrange
        var company = await AddCompanyAsync();
        var employee = EmployeeFactory.GetEmployeeInCompany(company);
        var substituteEmployee = EmployeeFactory.GetEmployeeInCompany(company);
        CompaniesDbContext.Companies.Update(company);
        await CompaniesDbContext.SaveChangesAsync();
        var command = new DeactivateEmployeeCommand(Guid.Empty, substituteEmployee.Id);
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"companies-module/employees/deactivate/{employee.Id.Value}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedEmployee = await GetEmployeeByIdAsync(employee.Id);
        updatedEmployee.IsActive.Value.ShouldBeFalse();
    }
    
    [Fact]
    public async Task DeactivateEmployee_GivenExistingEmployeeIdAndNotExistingSubstituteEmployeeId_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var company = await AddCompanyAsync();
        var employee = EmployeeFactory.GetEmployeeInCompany(company);
        CompaniesDbContext.Companies.Update(company);
        await CompaniesDbContext.SaveChangesAsync();
        var command = new DeactivateEmployeeCommand(Guid.Empty, Guid.NewGuid());
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"companies-module/employees/deactivate/{employee.Id.Value}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var updatedEmployee = await GetEmployeeByIdAsync(employee.Id);
        updatedEmployee.IsActive.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task DeactivateEmployee_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new DeactivateEmployeeCommand(Guid.Empty, Guid.NewGuid());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"companies-module/employees/deactivate/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task DeactivateEmployee_GivenAuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        var command = new DeactivateEmployeeCommand(Guid.Empty, Guid.NewGuid());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"companies-module/employees/deactivate/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    private async Task<Company> AddCompanyAsync()
    {
        var company = CompanyFactory.Get().Single();;
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        return company;
    }
    
    private Task<Employee> GetEmployeeByIdAsync(Guid id)
        => CompaniesDbContext
        .Employees
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id.Equals(id))!;
}