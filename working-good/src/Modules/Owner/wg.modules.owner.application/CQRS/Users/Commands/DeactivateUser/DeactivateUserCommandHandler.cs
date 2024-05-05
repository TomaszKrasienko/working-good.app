using wg.modules.owner.application.Events;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.owner.application.CQRS.Users.Commands.DeactivateUser;

internal sealed class DeactivateUserCommandHandler(
    IOwnerRepository ownerRepository,
    IMessageBroker messageBroker) : ICommandHandler<DeactivateUserCommand>
{
    public async Task HandleAsync(DeactivateUserCommand command, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetAsync();

        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }
        
        owner.DeactivateUser(command.Id);

        await ownerRepository.UpdateAsync(owner);
        await messageBroker.PublishAsync(new UserDeactivated(command.Id));
    }
}