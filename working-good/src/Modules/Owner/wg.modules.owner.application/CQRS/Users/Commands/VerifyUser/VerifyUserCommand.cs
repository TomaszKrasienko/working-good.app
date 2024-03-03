using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;

public record VerifyUserCommand(string Token) : ICommand;