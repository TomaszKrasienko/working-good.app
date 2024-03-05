using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Users.Commands.SignIn;

public sealed record SignInCommand(string Email, string Password) : ICommand;