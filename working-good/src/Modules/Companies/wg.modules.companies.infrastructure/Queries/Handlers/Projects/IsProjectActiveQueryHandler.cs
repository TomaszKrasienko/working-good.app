using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Projects.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Time;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Projects;

internal sealed class IsProjectActiveQueryHandler(
    CompaniesDbContext dbContext, IClock clock) : IQueryHandler<IsProjectActiveQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsProjectActiveQuery query, CancellationToken cancellationToken)
    {
        var projectsForEmployee = await dbContext
            .Companies
            .Include(x => x.Projects)
            .Where(x=> x.Projects.Any(y => y.Id.Equals(query.Id)))
            .ToListAsync(cancellationToken);
        
        return new IsExistsDto()
        {
            Value = projectsForEmployee.Any(x 
                => x.Projects.Any(y => y.PlannedFinish == null 
                                       || y.PlannedFinish.Value > clock.Now()))
        };
    }
}