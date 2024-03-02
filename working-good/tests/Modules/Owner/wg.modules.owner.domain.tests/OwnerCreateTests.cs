using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.User;
using Xunit;

namespace wg.modules.owner.domain.tests;

public sealed class OwnerCreateTests
{
    [Fact]
    public void Create_GivenEmptyName_ShouldThrowEmptyOwnerNameException()
    {
        //act
        var exception = Record.Exception(() => Owner.Create(string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyOwnerNameException>();
    }

    [Fact]
    public void Create_ForValidArguments_ShouldReturnOwnerWithFilledFields()
    {
        //arrange
        var name = "company_name";
        
        //act
        var result = Owner.Create(name);
        
        //assert
        result.ShouldNotBeNull();
        result.Name.Value.ShouldBe(name);
    }

    [Fact]
    public void AddUser_GivenFirstUserAsManager_ShouldAddToUsers()
    {
        //arrange
        var owner = Owner.Create("test_company_name");
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
        var owner = Owner.Create("test_company_name");
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
        var owner = Owner.Create("test_company_name");

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
        var owner = Owner.Create("test_company_name");

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
        var owner = Owner.Create("test_company_name");
        owner.AddUser(Guid.NewGuid(), "joe@doe.pl", "Joe", "Doe", "Pass123!", Role.Manager());

        //act
        owner.AddUser(userId, "new@user.pl", "Jan", "Bastian", "Pass123!",
            Role.User());
        
        //assert
        var user = owner.Users.FirstOrDefault(x => x.Id.Value == userId);
        user.ShouldNotBeNull();
    }
}