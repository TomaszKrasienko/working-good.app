using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignEmployee;

public sealed record AssignEmployeeCommand(Guid EmployeeId, Guid TicketId) : ICommand;