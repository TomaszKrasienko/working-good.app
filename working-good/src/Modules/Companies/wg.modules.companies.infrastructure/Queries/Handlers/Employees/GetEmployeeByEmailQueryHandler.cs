using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class GetEmployeeByEmailQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetEmployeeByEmailQuery, EmployeeDto>
{
    public async Task<EmployeeDto> HandleAsync(GetEmployeeByEmailQuery query, CancellationToken cancellationToken)
        => (await dbContext
                .Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == query.Email, cancellationToken))
            .AsDto();
}