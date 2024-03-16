using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;

internal sealed class AddUserToGroupCommandHandler(IOwnerRepository ownerRepository) 
    : ICommandHandler<AddUserToGroupCommand>
{
    public async Task HandleAsync(AddUserToGroupCommand command, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetAsync();
        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }
        owner.AddUserToGroup(command.GroupId, command.UserId);
        await ownerRepository.UpdateAsync(owner);
    }
}