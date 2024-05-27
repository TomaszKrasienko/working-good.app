using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External;

public sealed record EmployeeAdded(Guid Id, string Email) : IEvent;