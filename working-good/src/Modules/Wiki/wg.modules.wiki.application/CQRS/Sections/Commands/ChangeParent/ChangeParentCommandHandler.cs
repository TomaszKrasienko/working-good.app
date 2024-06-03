using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.wiki.application.CQRS.Sections.Commands.ChangeParent;

internal sealed class ChangeParentCommandHandler(
    ISectionRepository sectionRepository) : ICommandHandler<ChangeParentCommand>
{
    public async Task HandleAsync(ChangeParentCommand command, CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByIdAsync(command.SectionId, cancellationToken);
        if (section is null)
        {
            throw new SectionNotFoundException(command.SectionId);
        }

        Section parentSection = null;
        if (command.ParentSectionId is not null)
        {
            parentSection = await sectionRepository.GetByIdAsync(command.ParentSectionId.Value, cancellationToken);
            if (parentSection is null)
            {
                throw new ParentSectionNotFoundException(command.ParentSectionId.Value);
            }
        }

        section.ChangeParent(parentSection);
        await sectionRepository.UpdateAsync(section, cancellationToken);
    }
}