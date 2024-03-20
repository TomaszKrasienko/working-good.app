using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Companies;

internal sealed class GetCompanyByIdQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetCompanyByIdQuery, CompanyDto>
{
    public async Task<CompanyDto> HandleAsync(GetCompanyByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
            .Companies
            .Include(x => x.Employees)
            .Include(x => x.Projects)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?.AsDto();
}