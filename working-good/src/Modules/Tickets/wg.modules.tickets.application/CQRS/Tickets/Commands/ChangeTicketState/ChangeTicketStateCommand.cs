using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;

public sealed record ChangeTicketStateCommand(Guid TicketId, string Status) : ICommand;