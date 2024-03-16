using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;

public sealed record AddUserToGroupCommand(Guid GroupId, Guid UserId) : ICommand;