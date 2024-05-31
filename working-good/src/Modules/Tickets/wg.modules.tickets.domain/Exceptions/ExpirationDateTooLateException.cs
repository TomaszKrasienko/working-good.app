using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class ExpirationDateTooLateException()
    : WgException("Provided expiration date is too late");