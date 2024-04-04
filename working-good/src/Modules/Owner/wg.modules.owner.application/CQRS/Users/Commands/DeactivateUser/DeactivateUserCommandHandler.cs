using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Users.Commands.DeactivateUser;

internal sealed class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand>
{
    public Task HandleAsync(DeactivateUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}