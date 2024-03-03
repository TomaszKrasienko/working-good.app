using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Owners.Commands.ChangeOwnerName;

public sealed class ChangeOwnerNameCommandHandler(
    IOwnerRepository ownerRepository) : ICommandHandler<ChangeOwnerNameCommand>
{
    public async Task HandleAsync(ChangeOwnerNameCommand command, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetByIdAsync(command.Id);
        if (owner is null)
        {
            throw new OwnerNotFoundException(command.Id);
        }
        owner.ChangeName(command.Name);
        await ownerRepository.UpdateAsync(owner);
    }
}