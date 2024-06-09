using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Users.Commands.Edit;

public sealed record EditUserCommand(Guid Id, string Email, string FirstName, string LastName,
    string Role) : ICommand;