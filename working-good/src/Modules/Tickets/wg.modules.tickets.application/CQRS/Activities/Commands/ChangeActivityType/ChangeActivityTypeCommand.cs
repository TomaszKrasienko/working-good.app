using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Activities.Commands.ChangeActivityType;

public sealed record ChangeActivityTypeCommand(Guid Id) : ICommand;