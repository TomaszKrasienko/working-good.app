using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.tests.shared.Factories;
using Xunit;

namespace wg.modules.owner.domain.tests;

public sealed class OwnerTests
{
    [Fact]
    public void AddUser_GivenFirstUserAsManager_ShouldAddToUsers()
    {
        //arrange
        var owner = OwnerFactory.Get();
        Guid userId = Guid.NewGuid();
        
        //act
        owner.AddUser(userId, "test@test.pl", "Joe", "Doe",
            "Pass123!", Role.Manager());
        
        //assert
        var user = owner.Users.FirstOrDefault(x => x.Id.Value == userId);
        user.ShouldNotBeNull();
    }

    [Fact]
    public void AddUser_GivenFirstUserAsUser_ShouldThrowInvalidFirstUserRoleException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        Guid userId = Guid.NewGuid();
        
        //act
        var exception = Record.Exception( () => owner.AddUser(userId, "test@test.pl", "Joe", "Doe",
            "Pass123!", Role.User()));
        
        //assert
        exception.ShouldBeOfType<InvalidFirstUserRoleException>();
    }

    [Fact]
    public void AddUser_GivenExistingId_ShouldThrowUserAlreadyRegisteredException()
    {
        //arrange
        var owner = OwnerFactory.Get();

        var userId = Guid.NewGuid();
        owner.AddUser(userId, "joe@doe.pl", "Joe", "Doe", "Pass123!", Role.Manager());
        
        //act
        var exception = Record.Exception(() => owner.AddUser(userId, "new@user.pl", "Jan", "Bastian", "Pass123!",
            Role.User()));
        
        //assert
        exception.ShouldBeOfType<UserAlreadyRegisteredException>();
    }
    
    [Fact]
    public void AddUser_GivenExistingEmail_ShouldThrowUserAlreadyRegisteredException()
    {
        //arrange
        var owner = OwnerFactory.Get();

        var userId = Guid.NewGuid();
        owner.AddUser(userId, "joe@doe.pl", "Joe", "Doe", "Pass123!", Role.Manager());
        
        //act
        var exception = Record.Exception(() => owner.AddUser(userId, "new@user.pl", "Jan", "Bastian", "Pass123!",
            Role.User()));
        
        //assert
        exception.ShouldBeOfType<UserAlreadyRegisteredException>();
    }

    [Fact]
    public void AddUser_GivenUserRoleAndNotFirstUser_ShouldAddToUsers()
    {
        //arrange
        var userId = Guid.NewGuid();
        var owner = OwnerFactory.Get();
        owner.AddUser(Guid.NewGuid(), "joe@doe.pl", "Joe", "Doe", "Pass123!", Role.Manager());

        //act
        owner.AddUser(userId, "new@user.pl", "Jan", "Bastian", "Pass123!",
            Role.User());
        
        //assert
        var user = owner.Users.FirstOrDefault(x => x.Id.Value == userId);
        user.ShouldNotBeNull();
    }

    [Fact]
    public void VerifyUser_GivenExistingUser_ShouldChangeUserStateAndAddVerificationDate()
    {
        //arrange
        var owner = OwnerFactory.Get();
        owner.AddUser(Guid.NewGuid(), "test@test.pl", "Joe", "Doe", "Pass123!", Role.Manager());
        var userVerificationToken = owner.Users.Single().VerificationToken.Token;
        
        //act
        owner.VerifyUser(userVerificationToken, DateTime.Now);
        
        //assert
        var user = owner.Users.Single();
        user.State.Value.ShouldBe("Activated");
        user.VerificationToken.VerificationDate.ShouldBeNull();
    }

    [Fact]
    public void VerifyUser_GivenNotExistingUser_ShouldThrowVerificationTokenNotFoundException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        
        //act
        var exception = Record.Exception(() => owner.VerifyUser("invalid_token", DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<VerificationTokenNotFoundException>();
    }
}