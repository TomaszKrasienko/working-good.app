using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;

public sealed record AddOwnerCommand(Guid Id, string Name) : ICommand;