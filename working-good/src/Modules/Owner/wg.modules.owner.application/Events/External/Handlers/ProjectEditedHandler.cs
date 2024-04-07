using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events.External.Handlers;

internal sealed class ProjectEditedHandler(
    IOwnerRepository ownerRepository) : IEventHandler<ProjectEdited>
{
    public async Task HandleAsync(ProjectEdited @event)
    {
        var owner = await ownerRepository.GetAsync();
        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }

        owner.EditGroup(@event.Id, @event.Title);
        await ownerRepository.UpdateAsync(@owner);
    }
}