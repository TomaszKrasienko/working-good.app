using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.application.Exceptions;

public sealed class SubstituteEmployeeNotActiveException(Guid employeeId)
    :   WgException($"Employee with ID: {employeeId} for substitution is not active");