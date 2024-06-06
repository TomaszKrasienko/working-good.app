using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using wg.modules.wiki.application.CQRS.Notes.Queries;
using wg.modules.wiki.application.DTOs;
using wg.modules.wiki.infrastructure.DAL;
using wg.modules.wiki.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.wiki.infrastructure.Queries.Notes;

internal sealed class GetNoteByIdQueryHandler(
    WikiDbContext wikiDbContext) : IQueryHandler<GetNoteByIdQuery, NoteDto>
{
    public async Task<NoteDto> HandleAsync(GetNoteByIdQuery query, CancellationToken cancellationToken)
        => (await wikiDbContext
            .Notes
            .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?.AsDto();

}