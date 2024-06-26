using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events.External.Handlers;

internal sealed class ProjectAddedHandler(
    IOwnerRepository ownerRepository) : IEventHandler<ProjectAdded>
{
    public async Task HandleAsync(ProjectAdded @event)
    {
        var owner = await ownerRepository.GetAsync();
        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }
        
        owner.AddGroup(@event.Id, @event.Title);
        await ownerRepository.UpdateAsync(owner);
    }
}