using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events.External;

internal sealed record EmployeeDeactivated(Guid EmployeeId, Guid SubstituteEmployeeId) : IEvent;