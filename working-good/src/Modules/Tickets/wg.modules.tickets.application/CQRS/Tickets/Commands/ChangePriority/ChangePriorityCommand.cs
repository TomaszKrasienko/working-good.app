using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangePriority;

public record ChangePriorityCommand(Guid TicketId) : ICommand;