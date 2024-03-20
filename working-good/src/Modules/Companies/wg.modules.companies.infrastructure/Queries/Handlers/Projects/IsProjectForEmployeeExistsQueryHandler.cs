using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Projects.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Projects;

internal sealed class IsProjectForEmployeeExistsQueryHandler
    (CompaniesDbContext dbContext) : IQueryHandler<IsProjectForEmployeeExistsQuery, IsProjectExistsDto>
{
    public async Task<IsProjectExistsDto> HandleAsync(IsProjectForEmployeeExistsQuery query,
        CancellationToken cancellationToken)
        => new IsProjectExistsDto()
        {
            Value = await dbContext
                .Companies
                .Include(x => x.Employees)
                .Include(x => x.Projects)
                .AsNoTracking()
                .AnyAsync(c => c.Employees.Any(e => e.Id.Equals(query.EmployeeId))
                               && c.Projects.Any(p => p.Id.Equals(query.ProjectId)), cancellationToken)
        };
}