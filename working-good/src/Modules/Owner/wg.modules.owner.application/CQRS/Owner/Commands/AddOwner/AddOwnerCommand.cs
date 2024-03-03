using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Owner.Commands.AddOwner;

public record AddOwnerCommand(Guid Id, string Name) : ICommand;