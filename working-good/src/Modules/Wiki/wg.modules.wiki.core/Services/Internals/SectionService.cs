using wg.modules.wiki.core.DAL.Repositories.Abstractions;
using wg.modules.wiki.core.Entities;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.core.Services.Abstractions;
using wg.modules.wiki.core.Services.Commands;

namespace wg.modules.wiki.core.Services.Internals;

internal sealed class SectionService(
    ISectionRepository sectionRepository) : ISectionService
{
    public async Task AddAsync(AddSectionCommand command, CancellationToken cancellationToken)
    {
        if (await sectionRepository.IsNameExistsAsync(command.Name, cancellationToken))
        {
            throw new SectionNameAlreadyRegisteredException(command.Name);
        }

        Section parent = null;
        if (command.ParentId is not null)
        {
            parent = await sectionRepository.GetByIdAsync(command.ParentId!.Value, cancellationToken);
            if (parent is null)
            {
                throw new ParentSectionNotFoundException(command.ParentId!.Value);
            }
        }

        var section = Section.Create(command.Id, command.Name, parent);
        await sectionRepository
            .AddAsync(section, cancellationToken);
    }
}