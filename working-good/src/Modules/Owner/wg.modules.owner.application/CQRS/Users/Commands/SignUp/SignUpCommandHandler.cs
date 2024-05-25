using Microsoft.AspNetCore.Identity;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.Events;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.owner.application.CQRS.Users.Commands.SignUp;

internal sealed class SignUpCommandHandler(
    IOwnerRepository ownerRepository,
    IPasswordManager passwordManager,
    IMessageBroker messageBroker) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetAsync();
        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }

        var securedPassword = passwordManager.Secure(command.Password);
        var user = owner.AddUser(command.Id, command.Email, command.FirstName,
            command.LastName, securedPassword, command.Role);
        await ownerRepository.UpdateAsync(owner);
        await messageBroker.PublishAsync(new UserSignedUp(user.Id,user.Email, user.FullName.FirstName,
            user.FullName.LastName, user.VerificationToken.Token));
    }
}