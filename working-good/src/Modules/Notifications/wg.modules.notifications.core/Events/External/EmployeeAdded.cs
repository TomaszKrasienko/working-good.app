using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External;

public class EmployeeAdded(Guid Id, string Email) : IEvent;