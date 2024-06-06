using wg.modules.wiki.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.wiki.application.CQRS.Notes.Queries;

public sealed record GetNoteByIdQuery(Guid Id) : IQuery<NoteDto>;