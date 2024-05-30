using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Projects.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Projects;

internal sealed class GetProjectByIdQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetProjectByIdQuery, ProjectDto>
{
    public async Task<ProjectDto> HandleAsync(GetProjectByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
            .Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?.AsDto();
}