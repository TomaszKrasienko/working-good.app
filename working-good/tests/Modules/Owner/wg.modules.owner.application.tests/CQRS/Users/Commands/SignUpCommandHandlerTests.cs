using NSubstitute;
using Shouldly;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.tests.shared.Factories;
using wg.sharedForTests.Factories.Owners;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Users.Commands;

public sealed class SignUpCommandHandlerTests
{
    private Task Act(SignUpCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenSignUpCommandAndForExistingOwner_ShouldAddUserToOwnerAddUpdateByRepository()
    {
        //arrange
        var command = new SignUpCommand(Guid.NewGuid(), "joe@doe.pl", "Joe", "Doe",
            "Pass123!", Role.User());
        var securedPassword = Guid.NewGuid().ToString("N");
        
        var owner = OwnerFactory.Get();
        UserFactory.GetUserInOwner(owner, Role.Manager());
        _ownerRepository
            .GetAsync()
            .Returns(owner);

        _passwordManager
            .Secure(command.Password)
            .Returns(securedPassword);
        
        //act
        await Act(command);
        
        //assert
        await _ownerRepository
            .Received(1)
            .UpdateAsync(owner);
    }
    
    [Fact]
    public async Task HandleAsync_GivenSignUpCommandAndForNotExistingOwner_ShouldThrowOwnerNotFoundException()
    {
        //arrange
        var command = new SignUpCommand(Guid.NewGuid(), "joe@doe.pl", "Joe", "Doe",
            "Pass123!", Role.User());
        
        await _ownerRepository
            .GetAsync();
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly SignUpCommandHandler _handler;
    
    public SignUpCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _handler = new SignUpCommandHandler(_ownerRepository, _passwordManager);
    }
    #endregion
}