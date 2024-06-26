using Bogus.DataSets;
using Shouldly;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Exceptions;
using wg.shared.abstractions.Kernel.Exceptions;
using wg.tests.shared.Factories.Companies;
using Xunit;

namespace wg.modules.companies.domain.tests;

public sealed class CompanyTests
{
    [Fact]
    public void ChangeName_GivenNotEmptyName_ShouldChangeName()
    {
        //arrange
        var company = CompanyFactory.Get();
        var newName = "NewCompany name";
        
        //act
        company.ChangeName(newName);
        
        //assert
        company.Name.Value.ShouldBe(newName);
    }
    
    [Fact]
    public void ChangeName_GivenEmptyName_ShouldChangeName()
    {
        //arrange
        var company = CompanyFactory.Get();
        
        //act
        var exception = Record.Exception(() => company.ChangeName(string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyNameException>();
    }
    
    [Fact]
    public void ChangeSlaTime_GivenNotZeroTimeSpan_ShouldChangeTimeSpan()
    {
        //arrange
        var company = CompanyFactory.Get();
        var newSlaTime = TimeSpan.FromHours(1);
        
        //act
        company.ChangeSlaTime(newSlaTime);
        
        //assert
        company.SlaTime.Value.ShouldBe(newSlaTime);
    }
    
    [Fact]
    public void ChangeSlaTime_GivenNotZeroTimeSpan_ShouldThrowZeroSlaTimeException()
    {
        //arrange
        var company = CompanyFactory.Get();
        
        //act
        var exception = Record.Exception(() => company.ChangeSlaTime(TimeSpan.Zero));
        
        //assert
        exception.ShouldBeOfType<ZeroSlaTimeException>();
    }
    
    [Fact]
    public void AddEmployee_GivenNotExistingEmailWithValidEmailDomain_ShouldAddToUsers()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employeeId = Guid.NewGuid();
        var employeeEmail = $"joe.doe@{company.EmailDomain.Value}";
        var phoneNumber = "500 500 500";
        
        //act
        company.AddEmployee(employeeId, employeeEmail, phoneNumber);

        //assert
        var result = company.Employees.Single();
        result.Id.Value.ShouldBe(employeeId);
        result.Email.Value.ShouldBe(employeeEmail);
        result.PhoneNumber.Value.ShouldBe(phoneNumber);
    }

    [Fact]
    public void AddEmployee_GivenExistingEmail_ShouldThrowEmailAlreadyInUseException()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        
        //act
        var exception = Record.Exception(() => company.AddEmployee(Guid.NewGuid(),
            employee.Email, "500 500 500"));
        
        //assert
        exception.ShouldBeOfType<EmailAlreadyInUseException>();
    }

    [Fact]
    public void AddEmployee_GivenNotMatchingEmailToEmailDomain_ShouldThrowEmailNotMatchToEmailDomainException()
    {
        //arrange
        var company = CompanyFactory.Get();
        
        //act
        var exception = Record.Exception(() => company.AddEmployee(Guid.NewGuid(), "invalid@invalid.pl"));
        
        //assert
        exception.ShouldBeOfType<EmailNotMatchToEmailDomainException>();
    }
    
    [Fact]
    public void AddEmployee_GivenNoActiveCompany_ShouldThrowCompanyNotActiveException()
    {
        //arrange
        var company = CompanyFactory.Get();
        company.Deactivate();
        
        //act
        var exception = Record.Exception(() => company.AddEmployee(Guid.NewGuid(), "invalid@invalid.pl"));
        
        //assert
        exception.ShouldBeOfType<CompanyNotActiveException>();
    }
    
    [Fact]
    public void DeactivateEmployee_GivenExistingId_ShouldChangeIsActiveForFalse()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        
        //act
        company.DeactivateEmployee(employee.Id);
        
        //assert
        employee.IsActive.Value.ShouldBeFalse();
    }

    [Fact]
    public void DeactivateEmployee_GivenNotExistingId_ShouldThrowEmployeeNotFoundException()
    {
        //arrange
        var company = CompanyFactory.Get();
        
        //act
        var exception = Record.Exception(() => company.DeactivateEmployee(Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<EmployeeNotFoundException>();
    }

    [Fact]
    public void AddProject_GivenNotExistingTitle_ShouldAddProject()
    {
        //arrange
        var company = CompanyFactory.Get();
        var projectId = Guid.NewGuid();
        
        //act
        company.AddProject(projectId, "title", "My project description", DateTime.Now, DateTime.Now.AddMonths(3));
        
        //assert
        var project = company.Projects.FirstOrDefault(x => x.Id.Value == projectId);
        project.ShouldNotBeNull();
    }
    
    [Fact]
    public void AddProject_GivenExistingTitle_ShouldThrowProjectAlreadyRegisteredException()
    {
        //arrange
        var company = CompanyFactory.Get();
        var projectTitle = "My project title";
        company.AddProject(Guid.NewGuid(), projectTitle, "My project description", DateTime.Now, DateTime.Now.AddMonths(3));
        
        //act
        var exception = Record.Exception(() => company.AddProject(Guid.NewGuid(), projectTitle,
            "My project description", DateTime.Now, DateTime.Now.AddMonths(3)));
        
        //assert
        exception.ShouldBeOfType<ProjectAlreadyRegisteredException>();
    }
    
    [Fact]
    public void AddProject_GivenNoActiveCompany_ShouldThrowCompanyNotActiveException()
    {
        //arrange
        var company = CompanyFactory.Get();
        company.Deactivate();
        
        //act
        var exception = Record.Exception(() => company.AddProject(Guid.NewGuid(), "TestProjectTitle",
            "My project description", DateTime.Now, DateTime.Now.AddMonths(3)));
        
        //assert
        exception.ShouldBeOfType<CompanyNotActiveException>();
    }

    [Fact]
    public void EditProject_GivenExistingProject_ShouldEditProject()
    {
        //arrange
        var company = CompanyFactory.Get();
        var project = ProjectFactory.GetInCompany(company, false, false);
        var newTitle = "ProjectNewTitle";
        var newDescription = "ProjectNewDescription";
        var newPlannedStart = DateTime.Now.AddDays(1);
        var newPlannedFinish = DateTime.Now.AddDays(20);
        
        //act
        company.EditProject(project.Id, newTitle, newDescription, newPlannedStart,
            newPlannedFinish);
        
        //assert
        project.Title.Value.ShouldBe(newTitle);
        project.Description.Value.ShouldBe(newDescription);
        project.PlannedStart.Value.ShouldBe(newPlannedStart);
        project.PlannedFinish.Value.ShouldBe(newPlannedFinish);
    }

    [Fact]
    public void EditProject_GivenNotExistingProject_ShouldThrowProjectNotFoundException()
    {
        //arrange
        var company = CompanyFactory.Get();
        
        //act
        var exception = Record.Exception(() => company.EditProject(Guid.NewGuid(), "test", "test", null, null));
        
        //assert
        exception.ShouldBeOfType<ProjectNotFoundException>();
    }
}