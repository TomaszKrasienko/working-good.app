using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events;

public sealed record TicketCreated(Guid Id, int TicketNumber, string Subject, string Content, Guid? UserId, Guid? EmployeeId)
    : IEvent;