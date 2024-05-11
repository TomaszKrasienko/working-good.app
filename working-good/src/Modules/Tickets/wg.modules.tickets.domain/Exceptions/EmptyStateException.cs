using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class EmptyStatusException()
    : WgException("Status can not be empty");