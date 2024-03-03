using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Time;

namespace wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;

internal sealed class VerifyUserCommandHandler(IOwnerRepository ownerRepository,
    IClock clock)
    : ICommandHandler<VerifyUserCommand>
{
    public async Task HandleAsync(VerifyUserCommand command, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetAsync();
        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }
        
        owner.VerifyUser(command.Token, clock.Now());
        await ownerRepository.UpdateAsync(owner);
    }
}