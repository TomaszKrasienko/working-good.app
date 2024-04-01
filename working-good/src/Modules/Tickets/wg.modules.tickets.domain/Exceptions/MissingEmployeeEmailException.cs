using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class MissingEmployeeEmailException()
    :   WgException("Employee email is required");