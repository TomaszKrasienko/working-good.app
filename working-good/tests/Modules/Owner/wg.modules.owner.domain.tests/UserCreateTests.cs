using System.Security.Cryptography.X509Certificates;
using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.User;
using Xunit;

namespace wg.modules.owner.domain.tests;

public sealed class UserCreateTests
{
    private static User Act(Guid id, string email, string firstName, string lastName, string password, string role)
        => User.Create(id, (email ?? "test@test.pl"), (firstName ?? "Joe"), (lastName ?? "Doe"),
            (password ?? "Pass123!"), (role ?? Role.Manager()));
    
    [Fact]
    public void Create_GivenEmptyEmail_ShouldThrowEmptyUserEmailException()
    {
        //act
        var exception = Record.Exception( () => User.Create(Guid.NewGuid(), string.Empty, "Joe", "Doe", "Pass123!",
            Role.User()));
        //assert
        exception.ShouldBeOfType<EmptyUserEmailException>();
    }
    
    [Fact]
    public void Create_GivenInvalidEmail_ShouldThrowInvalidUserEmailException()
    {
        //act
        var exception = Record.Exception( () => User.Create(Guid.NewGuid(), "invalid_email", "Joe", "Doe", "Pass123!",
            Role.User()));
        //assert
        exception.ShouldBeOfType<InvalidUserEmailException>();
    }
    
    [Fact]
    public void Create_GivenEmptyFirstName_ShouldThrowEmptyFullNameFieldException()
    {
        //act
        var exception = Record.Exception( () => User.Create(Guid.NewGuid(), "test@test.pl", string.Empty, "Doe", "Pass123!",
            Role.User()));
        //assert
        exception.ShouldBeOfType<EmptyFullNameFieldException>();
    }
    
    [Fact]
    public void Create_GivenEmptyLastName_ShouldThrowEmptyFullNameFieldException()
    {
        //act
        var exception = Record.Exception( () => User.Create(Guid.NewGuid(), "test@test.pl", "Joe", string.Empty, "Pass123!",
            Role.User()));
        //assert
        exception.ShouldBeOfType<EmptyFullNameFieldException>();
    }
    
    [Fact]
    public void Create_GivenEmptyPassword_ShouldThrowEmptyPasswordException()
    {
        //act
        var exception = Record.Exception( () => User.Create(Guid.NewGuid(), "test@test.pl", "Joe", "Doe", string.Empty,
            Role.User()));
        //assert
        exception.ShouldBeOfType<EmptyPasswordException>();
    }
    
    [Fact]
    public void Create_GivenEmptyRole_ShouldThrowEmptyUserRoleException()
    {
        //act
        var exception = Record.Exception( () => User.Create(Guid.NewGuid(), "test@test.pl", "Joe", "Doe", "Pass123!",
            string.Empty));
        //assert
        exception.ShouldBeOfType<EmptyUserRoleException>();
    }
    
    [Fact]
    public void Create_GivenUnavailable_ShouldThrowUnavailableUserRoleException()
    {
        //act
        var exception = Record.Exception( () => User.Create(Guid.NewGuid(), "test@test.pl", "Joe", "Doe", "Pass123!",
            "invalid_role"));
        //assert
        exception.ShouldBeOfType<UnavailableUserRoleException>();
    }

    [Fact]
    public void Create_GivenValidArguments_ShouldReturnUserWithFilledProperties()
    {
        //arrange
        var id = Guid.NewGuid();
        var email = "test@test.pl";
        var firstName = "Joe";
        var lastName = "Doe";
        var password = "Pass123!";
        var role = Role.User();
        
        //act
        var user = User.Create(id, email, firstName, lastName, password, role);
        
        //assert
        user.ShouldNotBeNull();
        user.Id.Value.ShouldBe(id);
        user.Email.Value.ShouldBe(email);
        user.FullName.FirstName.ShouldBe(firstName);
        user.FullName.LastName.ShouldBe(lastName);
        user.Password.Value.ShouldBe(password);
        user.Role.Value.ShouldBe(role);
        user.VerificationToken?.Token.ShouldNotBeNullOrWhiteSpace();
        user.VerificationToken?.VerificationDate.ShouldBeNull();
    }
}