using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events;

public sealed record UserAssigned(Guid TicketId, int TicketNumber, Guid UserId) : IEvent;