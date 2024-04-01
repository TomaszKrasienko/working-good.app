using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class MissingUserEmailException() 
    : WgException("User email is required");