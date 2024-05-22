using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketExpirationDate;

public sealed record ChangeTicketExpirationDateCommand(Guid Id, DateTime ExpirationDate) : ICommand;