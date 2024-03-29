using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class GetEmployeeEmailQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetEmployeeEmailQuery, EmployeeEmailDto>
{
    public async Task<EmployeeEmailDto> HandleAsync(GetEmployeeEmailQuery query, CancellationToken cancellationToken)
        => new EmployeeEmailDto
        {
            Value = (await dbContext
                .Employees
                .Where(x => x.Id.Equals(query.Id))
                .Select(x => x.Email)
                .FirstOrDefaultAsync(cancellationToken))
        };
}