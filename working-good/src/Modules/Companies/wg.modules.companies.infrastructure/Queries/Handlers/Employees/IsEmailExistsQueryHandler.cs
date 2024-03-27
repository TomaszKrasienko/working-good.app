using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class GetEmployeeIdQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetEmployeeIdQuery, EmployeeIdDto>
{
    public async Task<EmployeeIdDto> HandleAsync(GetEmployeeIdQuery query, CancellationToken cancellationToken)
        => new EmployeeIdDto()
        {
            Value = await dbContext
                .Employees
                .Where(x => x.Email == query.Email)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken)
        };
}