using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Companies;

internal sealed class IsActiveCompanyExistsQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<IsActiveCompanyExistsQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsActiveCompanyExistsQuery query, CancellationToken cancellationToken)
        => new IsExistsDto()
        {
            Value = await dbContext
                .Companies
                .AsNoTracking()
                .AnyAsync(x
                    => x.Id.Equals(query.Id) && x.IsActive, cancellationToken)
        };
}