using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;

public sealed record ChangeTicketStatusCommand(Guid Id, string Status) : ICommand;