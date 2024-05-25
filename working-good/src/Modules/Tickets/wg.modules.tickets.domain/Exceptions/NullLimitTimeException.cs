using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Entities;

public sealed class NullLimitTimeException() : 
    WgException("Limit time can not be null for priority ticket");