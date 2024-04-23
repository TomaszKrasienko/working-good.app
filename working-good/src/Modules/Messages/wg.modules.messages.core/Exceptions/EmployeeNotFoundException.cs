using wg.shared.abstractions.Exceptions;

namespace wg.modules.messages.core.Exceptions;

public sealed class EmployeeNotFoundException(string email) 
    : WgException($"Employee with email: {email} has not been found");