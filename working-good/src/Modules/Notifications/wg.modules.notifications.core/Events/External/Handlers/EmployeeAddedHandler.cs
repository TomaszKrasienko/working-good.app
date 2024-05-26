using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class EmployeeAddedHandler : IEventHandler<EmployeeAdded>
{
    public Task HandleAsync(EmployeeAdded @event)
    {
        throw new NotImplementedException();
    }
}