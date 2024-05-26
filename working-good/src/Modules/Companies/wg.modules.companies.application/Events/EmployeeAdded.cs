using wg.shared.abstractions.Events;

namespace wg.modules.companies.application.Events;

public class EmployeeAdded(Guid Id, string Email) : IEvent;