using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.wiki.application.CQRS.Notes.Commands;

internal sealed class AddNoteCommandHandler : ICommandHandler<AddNoteCommand>
{
    public Task HandleAsync(AddNoteCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}