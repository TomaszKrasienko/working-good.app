using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events.External.Handlers;

internal sealed class ProjectAddedHandler : IEventHandler<ProjectAdded>
{
    private readonly IOwnerRepository _ownerRepository;

    public ProjectAddedHandler(IOwnerRepository ownerRepository)
        => _ownerRepository = ownerRepository;
    
    public async Task HandleAsync(ProjectAdded @event)
    {
        var owner = await _ownerRepository.GetAsync();
        if (owner is null)
        {
            throw new OwnerNotFoundException();
        }
        
        owner.AddGroup(@event.Id, @event.Title);
        await _ownerRepository.UpdateAsync(owner);
    }
}