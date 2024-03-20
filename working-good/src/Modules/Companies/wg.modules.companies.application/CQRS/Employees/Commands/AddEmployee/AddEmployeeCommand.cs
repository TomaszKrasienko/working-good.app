using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;

public sealed record AddEmployeeCommand(Guid CompanyId, Guid Id, 
    string Email, string PhoneNumber) : ICommand;