using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public sealed class EmployeeNotFoundException(Guid employeeId)
    : WgException($"Employee with ID: {employeeId} not found");