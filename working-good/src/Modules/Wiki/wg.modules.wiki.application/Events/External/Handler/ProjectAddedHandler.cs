using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.Events;

namespace wg.modules.wiki.application.Events.External.Handler;

internal sealed class ProjectAddedHandler(
    ISectionRepository sectionRepository) : IEventHandler<ProjectAdded>
{
    public async Task HandleAsync(ProjectAdded @event)
    {
        var isNameExists = await sectionRepository.IsNameExistsAsync(@event.Title, default);
        if (isNameExists)
        {
            return;
        }

        var section = Section.Create(Guid.NewGuid(), @event.Title);
        await sectionRepository.AddAsync(section, default);
    }
}