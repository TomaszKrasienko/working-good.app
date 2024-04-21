using Microsoft.EntityFrameworkCore;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Pagination;

namespace wg.modules.companies.infrastructure.Queries.Handlers.Companies;

internal sealed class GetCompaniesQueryHandler(
    CompaniesDbContext dbContext) : IQueryHandler<GetCompaniesQuery, PagedList<CompanyDto>>
{
    public Task<PagedList<CompanyDto>> HandleAsync(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        var results = dbContext
            .Companies
            .AsNoTracking()
            .AsEnumerable()
            .Where(x
                => (string.IsNullOrWhiteSpace(query.Name) || x.Name.Value.Contains(query.Name)))
            .Select(x => x.AsDto());
        return Task.FromResult(PagedList<CompanyDto>.ToPagedList(results.AsQueryable(), query.PageNumber, query.PageSize));
    }
}