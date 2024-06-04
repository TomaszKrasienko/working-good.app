using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.wiki.application.CQRS.Notes.Commands;

public sealed record AddNoteCommand(Guid Id, string Title, string Content, string OriginType = null,
    string OriginId = null) : ICommand;