using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.application.CQRS.Owners.Commands.ChangeOwnerName;

public sealed record ChangeOwnerNameCommand(Guid Id, string Name) : ICommand;