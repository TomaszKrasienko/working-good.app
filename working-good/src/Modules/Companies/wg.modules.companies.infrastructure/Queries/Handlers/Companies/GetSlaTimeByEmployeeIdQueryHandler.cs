using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Companies;

internal sealed class GetSlaTimeByEmployeeIdQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetSlaTimeByEmployeeIdQuery, SlaTimeDto>
{
    public async Task<SlaTimeDto> HandleAsync(GetSlaTimeByEmployeeIdQuery query, CancellationToken cancellationToken)
        => new SlaTimeDto()
        {
            Value = await dbContext
                .Companies
                .Include(x => x.Employees)
                .Where(x => x.Employees.Any(e => e.Id.Equals(query.Id)))
                .Select(x => x.SlaTime)
                .SingleOrDefaultAsync(cancellationToken)
        };
}