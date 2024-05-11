using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;

public sealed record AddTicketCommand(Guid Id, string Subject, string Content, Guid CreatedBy) : ICommand;