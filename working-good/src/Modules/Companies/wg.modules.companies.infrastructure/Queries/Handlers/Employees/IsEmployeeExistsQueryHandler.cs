using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class IsEmployeeExistsQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<IsEmployeeExistsQuery,IsEmployeeExistsDto>
{
    public async Task<IsEmployeeExistsDto> HandleAsync(IsEmployeeExistsQuery query, CancellationToken cancellationToken)
        => new IsEmployeeExistsDto()
        {
            Value = (await dbContext
                .Employees
                .AsNoTracking()
                .AnyAsync(e => e.Id.Equals(query.EmployeeId), cancellationToken))
        };


}