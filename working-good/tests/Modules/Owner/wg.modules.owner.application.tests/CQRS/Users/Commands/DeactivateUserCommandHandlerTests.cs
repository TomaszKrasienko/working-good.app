using NSubstitute;
using Shouldly;
using wg.modules.owner.application.CQRS.Users.Commands.DeactivateUser;
using wg.modules.owner.application.Events;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Factories.Owners;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Users.Commands;

public sealed class DeactivateUserCommandHandlerTests
{
    private Task Act(DeactivateUserCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    private async Task HandleAsync_GivenUseId_ShouldUpdateOwnerByRepositoryAndSendUserDeactivatedEvent()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        owner.AddUserToGroup(group.Id, user.Id);

        _ownerRepository
            .GetAsync()
            .Returns(owner);

        var command = new DeactivateUserCommand(user.Id);
        
        //act
        await Act(command);
        
        //assert
        user.State.Value.ShouldBe(State.Deactivate());
        
        await _ownerRepository
            .Received(1)
            .UpdateAsync(owner);

        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<UserDeactivated>(arg 
                => arg.UserId.Equals(user.Id)));
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingOwner_ShouldThrowOwnerNotFoundException()
    {
        //arrange
        var command = new DeactivateUserCommand(Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly ICommandHandler<DeactivateUserCommand> _handler;

    public DeactivateUserCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new DeactivateUserCommandHandler(_ownerRepository, _messageBroker);
    }
    #endregion
}
