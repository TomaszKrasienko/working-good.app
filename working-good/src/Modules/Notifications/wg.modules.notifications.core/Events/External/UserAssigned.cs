using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External;

public sealed record UserAssigned(Guid TicketId, int TicketNumber, Guid UserId) : IEvent;