using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Companies;

internal sealed class GetCompanyByEmployeeIdQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetCompanyByEmployeeIdQuery, CompanyDto>
{
    public async Task<CompanyDto> HandleAsync(GetCompanyByEmployeeIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
                .Companies
                .Include(x => x.Employees)
                .Include(x => x.Projects)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Employees
                    .Any(e => e.Id.Equals(query.EmployeeId)), cancellationToken))?
            .AsDto();
}