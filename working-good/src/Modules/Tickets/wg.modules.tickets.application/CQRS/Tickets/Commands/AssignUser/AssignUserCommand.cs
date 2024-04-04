using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;

public sealed record AssignUserCommand(Guid UserId, Guid TicketId) : ICommand;