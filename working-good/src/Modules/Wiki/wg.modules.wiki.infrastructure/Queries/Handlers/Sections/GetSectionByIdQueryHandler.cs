using Microsoft.EntityFrameworkCore;
using wg.modules.wiki.application.CQRS.Sections.Queries;
using wg.modules.wiki.application.DTOs;
using wg.modules.wiki.infrastructure.DAL;
using wg.modules.wiki.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.wiki.infrastructure.Queries.Handlers.Sections;

internal sealed class GetSectionByIdQueryHandler(
    WikiDbContext dbContext) : IQueryHandler<GetSectionByIdQuery, SectionDto>
{
    public async Task<SectionDto> HandleAsync(GetSectionByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
                .Sections
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(query.SectionId), cancellationToken)
            )?.AsDto();
}