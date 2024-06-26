using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;
using wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

[Collection("#1")]
public sealed class EmployeesControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetById_GivenExistingId_ShouldReturnEmployeeDto()
    {
        //arrange
        var company = await AddCompanyAsync();
        var employee = await AddEmployeeInCompanyAsync(company);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<EmployeeDto>($"companies-module/Employees/{employee.Id.Value}");

        //assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<EmployeeDto>();
    }
    
    [Fact]
    public async Task GetById_NotExistingId_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());

        //act
        var result = await HttpClient.GetAsync($"companies-module/Employees/{Guid.NewGuid()}");

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var result = await HttpClient.GetAsync($"companies-module/Employees/{Guid.NewGuid()}");

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetActiveById_GivenExistingActiveEmployee_ShouldReturnEmployeeDto()
    {
        //arrange
        var company = await AddCompanyAsync();
        var employee = await AddEmployeeInCompanyAsync(company);
        
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<EmployeeDto>($"companies-module/Employees/{employee.Id.Value}/active");
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(employee.Id.Value);
    }
    
    [Fact]
    public async Task GetActiveById_GivenNotExistingActiveEmployee_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        var company = await AddCompanyAsync();
        var employee = await AddEmployeeInCompanyAsync(company);
        employee.Deactivate();
        CompaniesDbContext.Companies.Update(company);
        await CompaniesDbContext.SaveChangesAsync();
        
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"companies-module/Employees/{employee.Id.Value}/active");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetActiveById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"companies-module/Employees/{Guid.NewGuid()}/active");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task IsEmployeeActiveById_GivenExistingActiveEmployeeId_ShouldReturnIsExistsDtoWithTrueValue()
    {
        //arrange
        var company = await AddCompanyAsync();
        var employee = await AddEmployeeInCompanyAsync(company);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"companies-module/employees/{employee.Id.Value}/is-active");
        
        //assert
        result.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task IsEmployeeActiveById_GivenNotExistingActiveEmployeeId_ShouldReturnIsExistsDtoWithFalseValue()
    {
        //arrange
        var company = await AddCompanyAsync();
        var employee = await AddEmployeeInCompanyAsync(company);
        employee.Deactivate();
        CompaniesDbContext.Companies.Update(company);
        await CompaniesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"companies-module/employees/{employee.Id.Value}/is-active");
        
        //assert
        result.Value.ShouldBeFalse();
    }

    [Fact]
    public async Task IsEmployeeActiveById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"companies-module/employees/{Guid.NewGuid()}/is-active");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

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
        var employees = EmployeeFactory.GetInCompany(company, 2).ToList();
        var employee = employees[0];
        var substituteEmployee = employees[1];
        
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
        var employee = await AddEmployeeInCompanyAsync(company);
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
        var company = CompanyFactory.Get();
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        return company;
    }

    private async Task<Employee> AddEmployeeInCompanyAsync(Company company)
    {
        var employee = EmployeeFactory.GetInCompany(company);
        CompaniesDbContext.Companies.Update(company);
        await CompaniesDbContext.SaveChangesAsync();
        return employee;
    }
    
    private Task<Employee> GetEmployeeByIdAsync(Guid id)
        => CompaniesDbContext
        .Employees
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id.Equals(id))!;
}