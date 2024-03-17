using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class EmptyStateException()
    : WgException("State can not be empty");