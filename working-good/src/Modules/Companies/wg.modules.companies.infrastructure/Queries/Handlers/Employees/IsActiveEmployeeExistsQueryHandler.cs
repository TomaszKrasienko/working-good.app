using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class IsActiveEmployeeExistsQueryHandler(
    CompaniesDbContext companiesDbContext) : IQueryHandler<IsActiveEmployeeExistsQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsActiveEmployeeExistsQuery query, CancellationToken cancellationToken)
        => new IsExistsDto()
        {
            Value = await companiesDbContext.Employees.AnyAsync(x => x.IsActive, cancellationToken)
        };
}