using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketStatus;

public sealed record ChangeTicketStatusCommand(Guid TicketId, string Status) : ICommand;