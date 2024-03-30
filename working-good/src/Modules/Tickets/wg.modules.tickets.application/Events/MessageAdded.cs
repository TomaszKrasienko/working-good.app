using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events;

public sealed record MessageAdded(int TicketNumber, string Subject, string Content, params string[] Recipients) : IEvent;