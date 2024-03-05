using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Users.Commands.SignIn;

internal sealed class SignInCommandHandler(
    IOwnerRepository ownerRepository) : ICommandHandler<SignInCommand>
{
    public Task HandleAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}