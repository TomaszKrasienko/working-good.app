using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External;

public sealed record MessageAdded(int TicketNumber, string Subject, string Content, params string[] Recipients) : IEvent;