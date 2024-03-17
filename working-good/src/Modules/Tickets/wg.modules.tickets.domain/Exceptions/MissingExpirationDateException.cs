using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class MissingExpirationDateException(bool priority) 
    : WgException($"With priority set up as {priority} expiration date is required");