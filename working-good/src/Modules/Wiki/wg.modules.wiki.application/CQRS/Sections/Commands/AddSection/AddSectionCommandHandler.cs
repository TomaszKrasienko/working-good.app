using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.wiki.application.CQRS.Sections.Commands;

internal sealed class AddSectionCommandHandler(
    ISectionRepository sectionRepository) : ICommandHandler<AddSectionCommand>
{
    public async Task HandleAsync(AddSectionCommand command, CancellationToken cancellationToken)
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