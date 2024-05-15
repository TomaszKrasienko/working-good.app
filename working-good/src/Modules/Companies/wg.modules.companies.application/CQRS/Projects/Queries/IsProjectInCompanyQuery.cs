using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.application.CQRS.Projects.Queries;

public sealed record IsProjectInCompanyQuery(Guid EmployeeId, Guid ProjectId) : IQuery<IsExistsDto>;