using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.application.CQRS.Companies.Queries;

public sealed record GetCompanyByEmployeeIdQuery(Guid EmployeeId) : IQuery<CompanyDto>;