using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.application.Strategies.Origins;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.domain.Exceptions;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.wiki.application.CQRS.Notes.Commands;

internal sealed class AddNoteCommandHandler(
    ISectionRepository sectionRepository,
    IEnumerable<IOriginCheckingStrategy> originCheckingStrategies) : ICommandHandler<AddNoteCommand>
{
    public async Task HandleAsync(AddNoteCommand command, CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByIdAsync(command.SectionId, cancellationToken);
        if (section is null)
        {
            throw new SectionNotFoundException(command.Id);
        }

        var originCheckingStrategy = originCheckingStrategies.SingleOrDefault(x
            => x.CanByApply(command.OriginType));

        if (originCheckingStrategy is null)
        {
            throw new OriginTypeNoteAvailableException(command.OriginType);
        }

        var isOriginExist = await originCheckingStrategy.IsExists(command.OriginId);
        if (!isOriginExist)
        {
            throw new OriginDoesNotExistException(command.OriginId, command.OriginType);
        }
    }
}