using Shouldly;
using wg.modules.companies.domain.Entities;
using wg.shared.abstractions.Kernel.Exceptions;
using Xunit;

namespace wg.modules.companies.domain.tests;

public sealed class EmployeeCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnEmployeeWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var email = "joe@doe.pl";
        var phoneNumber = "500 500 500";
        
        //act
        var result = Employee.Create(id, email, phoneNumber);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Email.Value.ShouldBe(email);
        result.PhoneNumber.Value.ShouldBe(phoneNumber);
    }

    [Fact]
    public void Create_GivenEmptyEmailShouldThrowEmptyEmailException()
    {
        //act
        var exception = Record.Exception(() => Employee.Create(Guid.NewGuid(), string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyEmailException>();
    }
}