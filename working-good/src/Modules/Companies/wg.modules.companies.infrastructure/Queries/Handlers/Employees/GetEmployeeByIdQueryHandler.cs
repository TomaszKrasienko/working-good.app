using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class GetEmployeeByIdQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    public async Task<EmployeeDto> HandleAsync(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
                .Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?
            .AsDto();
}