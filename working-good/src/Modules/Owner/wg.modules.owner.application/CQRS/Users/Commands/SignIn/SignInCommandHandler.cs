using wg.modules.owner.application.Auth;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Time;

namespace wg.modules.owner.application.CQRS.Users.Commands.SignIn;

internal sealed class SignInCommandHandler(
    IOwnerRepository ownerRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    IClock clock,
    ITokenStorage tokenStorage) : ICommandHandler<SignInCommand>
{
    public Task HandleAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}