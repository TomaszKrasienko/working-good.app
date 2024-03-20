using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Companies;

internal sealed class GetSlaTimeByEmployeeIdQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetSlaTimeByEmployeeIdQuery, CompanySlaTimeDto>
{
    public async Task<CompanySlaTimeDto> HandleAsync(GetSlaTimeByEmployeeIdQuery query,
        CancellationToken cancellationToken)
        => (await dbContext
                .Companies
                .Include(x => x.Employees)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Employees.Any(x => x.Id.Equals(query.EmployeeId)), cancellationToken))
            .AsSlaTimeDto();
}