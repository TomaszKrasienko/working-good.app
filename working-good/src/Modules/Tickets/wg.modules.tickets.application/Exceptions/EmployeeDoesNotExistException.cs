using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class EmployeeDoesNotExistException(Guid id)
    : WgException($"Employee with ID: {id} does not exist");