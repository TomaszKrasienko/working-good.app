using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class UserAssignedHandler : IEventHandler<UserAssigned>
{
    public Task HandleAsync(UserAssigned @event)
    {
        throw new NotImplementedException();
    }
}