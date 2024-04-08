using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;

public sealed record DeactivateEmployeeCommand(Guid Id, Guid SubstitutionEmployeeId) : ICommand;