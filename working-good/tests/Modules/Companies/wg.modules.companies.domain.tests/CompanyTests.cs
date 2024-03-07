using Shouldly;
using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.tests.shared.Factories;
using Xunit;

namespace wg.modules.companies.domain.tests;

public sealed class CompanyTests
{
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
        var employee = EmployeeFactory.GetEmployeeInCompany(company);
        
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
}