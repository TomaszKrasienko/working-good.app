using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class IsEmailExistsQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<IsEmailExistsQuery, IsEmailExistsDto>
{
    public async Task<IsEmailExistsDto> HandleAsync(IsEmailExistsQuery query, CancellationToken cancellationToken)
        => new IsEmailExistsDto()
        {
            Value = await dbContext
                .Employees
                .AnyAsync(x => x.Email == query.Email, cancellationToken)
        };
}