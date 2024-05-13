using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events;

public sealed record EmployeeAssigned(Guid TicketId, int TicketNumber, Guid EmployeeId) : IEvent;