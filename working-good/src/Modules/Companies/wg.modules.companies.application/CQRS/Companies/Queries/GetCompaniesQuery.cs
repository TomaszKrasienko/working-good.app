using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Pagination;

namespace wg.modules.companies.application.CQRS.Companies.Queries;

public sealed record GetCompaniesQuery : PaginationDto, IQuery<PagedList<CompanyDto>>
{
    public string Name { get; init; }
}