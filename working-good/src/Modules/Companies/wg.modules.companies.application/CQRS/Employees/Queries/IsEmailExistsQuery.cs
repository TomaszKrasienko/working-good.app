using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.application.CQRS.Employees.Queries;

public record IsEmailExistsQuery(string Email) : IQuery<IsEmployeeExistsDto>;