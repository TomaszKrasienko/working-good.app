using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Employees;

internal sealed class GetActiveEmployeeByIdQueryHandler(
    CompaniesDbContext companiesDbContext) : IQueryHandler<GetActiveEmployeeByIdQuery, EmployeeDto>
{
    public async Task<EmployeeDto> HandleAsync(GetActiveEmployeeByIdQuery query, CancellationToken cancellationToken)
        => (await companiesDbContext
            .Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x
                => x.Id.Equals(query.Id)
                   && x.IsActive, cancellationToken))?.AsDto();
}