using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class InvalidNowTimeForChangingPriorityException()
    : WgException("Provided date as \"now\" is invalid");