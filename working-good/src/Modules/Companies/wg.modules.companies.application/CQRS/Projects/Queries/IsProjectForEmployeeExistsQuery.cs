using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.application.CQRS.Projects.Queries;

public sealed record IsProjectForEmployeeExistsQuery(Guid EmployeeId, Guid ProjectId) : IQuery<IsProjectExistsDto>;