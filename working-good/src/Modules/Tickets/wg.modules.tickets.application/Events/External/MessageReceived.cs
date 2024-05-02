using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events.External;

public sealed record MessageReceived(string Sender, string Subject, string Content, string CreatedBy, DateTime CreatedAt,
    Guid AssignedEmployee, int? TicketNumber) : IEvent;