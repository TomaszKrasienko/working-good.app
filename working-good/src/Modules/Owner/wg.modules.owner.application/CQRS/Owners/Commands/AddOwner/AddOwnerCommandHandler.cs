using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;

internal sealed class AddOwnerCommandHandler(
    IOwnerRepository ownerRepository) : ICommandHandler<AddOwnerCommand>
{
    public async Task HandleAsync(AddOwnerCommand command, CancellationToken cancellationToken)
    {
        if (await ownerRepository.ExistsAsync())
        {
            throw new OwnerAlreadyExistsException();
        }

        var owner = Owner.Create(command.Id, command.Name);
        await ownerRepository.AddAsync(owner);
    }
}