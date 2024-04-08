using wg.shared.abstractions.Events;

namespace wg.modules.companies.application.Events;

internal sealed record EmployeeDeactivated(Guid EmployeeId, Guid SubstituteEmployeeId) : IEvent;