using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class NullLimitTimeException() : 
    WgException("Limit time can not be null for priority ticket");