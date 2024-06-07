using Shouldly;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.tests.shared.Factories.Companies;
using Xunit;

namespace wg.modules.companies.infrastructure.tests.Queries.Mappers;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsDto_GivenCompanyWithEmployeeAndProject_ShouldReturnCompanyDto()
    {
        //arrange
        var company = CompanyFactory.Get();
        EmployeeFactory.GetInCompany(company);
        ProjectFactory.GetInCompany(company, true, true);
        
        //act
        var result = company.AsDto();
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(company.Id.Value);
        result.Name.ShouldBe(company.Name.Value);
        result.SlaTime.ShouldBe(company.SlaTime.Value);
        result.EmailDomain.ShouldBe(company.EmailDomain.Value);
        result.Employees.ShouldNotBeEmpty();
        result.Projects.ShouldNotBeEmpty();
    }

    [Fact]
    public void AsDto_GivenCompany_ShouldReturnCompanyDto()
    {
        //arrange
        var company = CompanyFactory.Get();
        
        //act
        var result = company.AsDto();
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(company.Id.Value);
        result.Name.ShouldBe(company.Name.Value);
        result.SlaTime.ShouldBe(company.SlaTime.Value);
        result.EmailDomain.ShouldBe(company.EmailDomain.Value);
        result.Employees.ShouldBeEmpty();
        result.Projects.ShouldBeEmpty();
    }
    
    [Fact]
    public void AsDto_GivenEmployee_ShouldReturnEmployeeDto()
    {
        //arrange
        var employee = EmployeeFactory.Get().Single();
        
        //act
        var result = employee.AsDto();
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(employee.Id.Value);
        result.Email.ShouldBe(employee.Email.Value);
        result.PhoneNumber.ShouldBe(employee.PhoneNumber.Value);
        result.IsActive.ShouldBe(employee.IsActive.Value);
    }

    [Fact]
    public void AsDto_GivenProject_ShouldReturnProjectDto()
    {
        //arrange
        var company = CompanyFactory.Get();
        var project = ProjectFactory.GetInCompany(company, true, true);
        
        //act
        var result = project.AsDto();
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(project.Id.Value);
        result.Title.ShouldBe(project.Title.Value);
        result.Description.ShouldBe(project.Description.Value);
        result.PlannedStart.ShouldBe(project.PlannedStart.Value);
        result.PlannedFinish.ShouldBe(project.PlannedFinish.Value);
    }
}