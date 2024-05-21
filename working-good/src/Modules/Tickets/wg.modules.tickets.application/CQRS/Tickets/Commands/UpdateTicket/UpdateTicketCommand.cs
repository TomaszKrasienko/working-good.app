using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.UpdateTicket;

public sealed record UpdateTicketCommand(Guid Id, string Subject, string Content) : ICommand;