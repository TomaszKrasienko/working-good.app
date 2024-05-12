using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class ActiveEmployeeNotFoundException(Guid id)
    : WgException($"Active employee with ID: {id} does not exist");