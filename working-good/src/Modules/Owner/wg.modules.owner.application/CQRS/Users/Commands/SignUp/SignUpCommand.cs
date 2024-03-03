using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Users.Commands.SignUp;

public sealed record SignUpCommand(Guid Id, string Email, string FirstName, string LastName,
    string Password, string Role) : ICommand;