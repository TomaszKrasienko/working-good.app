using Microsoft.SqlServer.Server;
using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Factories.Owners;
using Xunit;

namespace wg.modules.owner.domain.tests;

public sealed class OwnerTests
{
    [Fact]
    public void ChangeName_GivenValidName_ShouldChangeName()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var newName = Guid.NewGuid().ToString("N");

        //act
        owner.ChangeName(newName);

        //assert
        owner.Name.Value.ShouldBe(newName);
    }

    [Fact]
    public void AddUser_GivenFirstUserAsManager_ShouldAddToUsersAndReturnUser()
    {
        //arrange
        var owner = OwnerFactory.Get();
        Guid userId = Guid.NewGuid();

        //act
        var user = owner.AddUser(userId, "test@test.pl", "Joe", "Doe",
            "Pass123!", Role.Manager());

        //assert
        user.ShouldNotBeNull();
    }

    [Fact]
    public void AddUser_GivenFirstUserAsUser_ShouldThrowInvalidFirstUserRoleException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        Guid userId = Guid.NewGuid();

        //act
        var exception = Record.Exception(() => owner.AddUser(userId, "test@test.pl", "Joe", "Doe",
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
        user.State.Value.ShouldBe("Active");
        user.VerificationToken.VerificationDate.ShouldNotBeNull();
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

    [Fact]
    public void EditUser_GivenExistingUser_ShouldChangeUser()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var users = UserFactory.GetInOwner(owner, Role.Manager(), 2).ToList();
        var email = "new@test.pl";
        var firstName = "NewFirstName";
        var lastName = "NewLastName";
        var role = Role.User();

        //act
        owner.EditUser(users[0].Id, email, firstName, lastName, role);

        //assert
        users[0].Email.Value.ShouldBe(email);
        users[0].FullName.FirstName.ShouldBe(firstName);
        users[0].FullName.LastName.ShouldBe(lastName);
        users[0].Role.Value.ShouldBe(role);
    }

    [Fact]
    public void EditUser_GivenFirstUserAndUserRole_ShouldThrowInvalidFirstUserRoleException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());

        //act
        var exception = Record.Exception(() => owner.EditUser(user.Id, "new@test.pl", "NewFirstName",
            "NewLastName", Role.User()));

        //assert
        exception.ShouldBeOfType<InvalidFirstUserRoleException>();
    }

    [Fact]
    public void EditUser_GivenNotExistingUser_ShouldThrowUserNotFoundException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        
        //act
        var exception = Record.Exception(() => owner.EditUser(
            Guid.NewGuid(), "new@test.pl", "NewName", "NewLastName", Role.Manager()));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }

    [Fact]
    public void IsUserActive_GivenExistingAndVerifiedUser_ShouldReturnTrue()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        owner.VerifyUser(user.VerificationToken.Token, DateTime.Now);
        
        //act
        var result = owner.IsUserActive(user.Email);
        
        //assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void IsUserActive_GivenNotExistingUser_ShouldReturnFalse()
    {
        //arrange
        var owner = OwnerFactory.Get();
        
        //act
        var result = owner.IsUserActive("inveli@email.pl");
        
        //assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsUserActive_GivenNotActiveUser_ShouldReturnFalse()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        
        //act
        var result = owner.IsUserActive(user.Email);
        
        //assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void AddGroup_GivenNotExistingTitle_ShouldAddToGroup()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var id = Guid.NewGuid();
        var title = "Group";
        
        //act
        owner.AddGroup(id, title);
        
        //assert
        owner.Groups.Any(x => x.Id.Equals(id)).ShouldBeTrue();
    }

    [Fact]
    public void AddGroup_GivenExistingTitle_ShouldThrowGroupAlreadyExistsException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var title = "Group";
        owner.AddGroup(Guid.NewGuid(), title);
        
        //act
        var exception = Record.Exception(() => owner.AddGroup(Guid.NewGuid(), title));
        
        //assert
        exception.ShouldBeOfType<GroupAlreadyExistsException>();
    }

    [Fact]
    public void AddUserToGroup_ForExistingUserAndExistingGroup_ShouldAddUserToGroup()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        
        //act
        owner.AddUserToGroup( group.Id, user.Id);
        
        //assert
        group.Users.Any(x => x.Id.Equals(user.Id)).ShouldBeTrue();
    }
    
    
    [Fact]
    public void AddUserToGroup_ForNotExistingUserAndExistingGroup_ShouldUserNotFoundException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var group = GroupFactory.GetInOwner(owner);
        
        //act
        var exception = Record.Exception(() => owner.AddUserToGroup(group.Id, Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }
    
    [Fact]
    public void AddUserToGroup_ForExistingUserAndNotExistingGroup_ShouldThrowGroupNotFoundException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        
        //act
        var exception = Record.Exception(() => owner.AddUserToGroup( Guid.NewGuid(), user.Id));
        
        //assert
        exception.ShouldBeOfType<GroupNotFoundException>();
    }

    [Fact]
    public void AddUserToGroup_ForAlreadyRegisteredInGroupUser_ShouldThrowUserAlreadyInGroupException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        owner.AddUserToGroup( group.Id, user.Id);
        
        //act
        var exception = Record.Exception(() => owner.AddUserToGroup(group.Id, user.Id));
        
        //assert
        exception.ShouldBeOfType<UserAlreadyInGroupException>();
    }

    [Fact]
    public void EditGroup_GivenExistingGroupId_ShouldChangeGroupTitle()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var group = GroupFactory.GetInOwner(owner);
        var newTitle = "TestNewTitle";
        
        //act
        owner.EditGroup(group.Id, newTitle);
        
        //assert
        var updatedGroup = owner.Groups.FirstOrDefault(x => x.Id.Equals(group.Id));
        updatedGroup!.Title.Value.ShouldBe(group.Title);
    }
    
    [Fact]
    public void EditGroup_GivenNotExistingGroupId_ShouldThrowGroupNotFoundException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        
        //act
        var exception = Record.Exception( () =>  owner.EditGroup(Guid.NewGuid(), "TestNewTitle"));
        
        //assert
        exception.ShouldBeOfType<GroupNotFoundException>();
    }
    
    [Fact]
    public void DeactivateUser_ExistingUser_ShouldChangeUserStateAndRemoveUserFromGroups()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var groups = GroupFactory.GetInOwner(owner, 2).ToList();
        owner.AddUserToGroup(groups[0].Id, user.Id);
        owner.AddUserToGroup(groups[1].Id, user.Id);
        
        //act
        owner.DeactivateUser(user.Id);

        //assert
        groups[0].Users.Any(x => x.Id.Equals(user.Id)).ShouldBeFalse();
        groups[1].Users.Any(x => x.Id.Equals(user.Id)).ShouldBeFalse();
    }
    
    [Fact]
    public void DeactivateUser_GivenNotExistingUserId_ShouldThrowUserNotFoundException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        
        //act
        var exception = Record.Exception(() => owner.DeactivateUser(Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }
}