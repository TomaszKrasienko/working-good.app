using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Users.Commands.DeactivateUser;

public record DeactivateUserCommand(Guid Id, Guid? SubstituteUserId) : ICommand;