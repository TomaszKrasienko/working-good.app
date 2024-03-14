using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events.External.Handlers;

internal sealed class ProjectAddedHandler : IEventHandler<ProjectAdded>
{
    public Task HandleAsync(ProjectAdded @event)
    {
        throw new NotImplementedException();
    }
}