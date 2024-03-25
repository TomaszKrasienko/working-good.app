using wg.shared.abstractions.Events;

namespace wg.modules.messages.core.Events;

public sealed record MessageReceived(string Sender, string Subject, string Content, DateTime CreatedAt,
    int? TicketNumber) : IEvent;