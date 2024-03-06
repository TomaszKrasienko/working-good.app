using wg.modules.owner.application.Auth;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Exceptions;
using wg.shared.abstractions.Time;

namespace wg.modules.owner.application.CQRS.Users.Commands.SignIn;

internal sealed class SignInCommandHandler(
    IOwnerRepository ownerRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage,
    IClock clock) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetAsync();
        if (!owner.IsUserActive(command.Email))
        {
            throw new UserIsNotActiveException(command.Email);
        }

        var user = owner.Users.Single(x => x.Email == command.Email);
        if (!passwordManager.VerifyPassword(user.Password, command.Password))
        {
            throw new IncorrectPasswordException(command.Password);
        }

        var jwt = authenticator.CreateToken(user.Id.ToString(), user.Role);
        tokenStorage.Set(jwt);
    }
}

public sealed class IncorrectPasswordException(string email)
    : WgAuthException($"Password for user with email {email} is incorrect");

public sealed class UserIsNotActiveException(string email) 
    : WgAuthException($"User with email: {email} is not active");