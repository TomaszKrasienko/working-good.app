using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.Events;

namespace wg.modules.wiki.application.Events.External.Handler;

internal sealed class CompanyAddedHandler(
    ISectionRepository sectionRepository) : IEventHandler<CompanyAdded>
{
    public async Task HandleAsync(CompanyAdded @event)
    {
        var isNameExists = await sectionRepository.IsNameExistsAsync(@event.Name, default);
        if (isNameExists)
        {
            return;
        }

        var section = Section.Create(Guid.NewGuid(), @event.Name);
        await sectionRepository.AddAsync(section, default);
    }
}