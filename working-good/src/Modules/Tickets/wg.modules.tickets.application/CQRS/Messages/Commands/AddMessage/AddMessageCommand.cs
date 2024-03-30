using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;

public sealed record AddMessageCommand(Guid Id, Guid UserId, string Content, Guid TicketId) : ICommand;