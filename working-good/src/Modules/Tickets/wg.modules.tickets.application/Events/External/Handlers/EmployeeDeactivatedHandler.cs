using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events.External.Handlers;

internal sealed class EmployeeDeactivatedHandler : IEventHandler<EmployeeDeactivated>
{
    public Task HandleAsync(EmployeeDeactivated @event)
    {
        throw new NotImplementedException();
    }
}