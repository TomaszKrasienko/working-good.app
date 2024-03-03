using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Owner.Commands.AddOwner;

internal sealed class AddOwnerCommandHandler(
    IOwnerRepository ownerRepository) : ICommandHandler<AddOwnerCommand>
{
    public Task HandleAsync(AddOwnerCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}