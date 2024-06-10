using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.owner.application.CQRS.Users.Commands.Edit;

internal sealed class EditUserCommandHandler(
    IOwnerRepository ownerRepository,
    IMessageBroker messageBroker) : ICommandHandler<EditUserCommand>
{
    public async Task HandleAsync(EditUserCommand command, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetAsync();
        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }
        
        owner.EditUser(command.Id, command.Email, command.FirstName, command.LastName, command.Role);
        await ownerRepository.UpdateAsync(owner);
        
    }
}