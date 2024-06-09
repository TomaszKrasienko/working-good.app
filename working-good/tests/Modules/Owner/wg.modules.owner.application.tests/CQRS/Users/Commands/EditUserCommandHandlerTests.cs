using NSubstitute;
using Shouldly;
using wg.modules.owner.application.CQRS.Users.Commands.Edit;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.Exceptions;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Factories.Owners;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Users.Commands;

public sealed class EditUserCommandHandlerTests
{
    private Task Act(EditUserCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingUser_ShouldUpdateUserAndUpdateOwnerByRepository()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.User());

        _ownerRepository
            .GetAsync()
            .Returns(owner);

        var command = new EditUserCommand(user.Id, "new@email.pl", "New", "Name",
            Role.Manager());
        
        //act
        await Act(command);
        
        //assert
        user.Email.Value.ShouldBe(command.Email);
        user.FullName.FirstName.ShouldBe(command.FirstName);
        user.FullName.LastName.ShouldBe(command.LastName);
        user.Role.Value.ShouldBe(command.Role);
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingOwner_ShouldThrowOwnerNotFound()
    {
        //arrange
        var command = new EditUserCommand(Guid.NewGuid(), "new@email.pl", "New", "Name",
            Role.Manager());
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly ICommandHandler<EditUserCommand> _handler;
    
    public EditUserCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new EditUserCommandHandler(_ownerRepository, _messageBroker);
    }
    #endregion
}