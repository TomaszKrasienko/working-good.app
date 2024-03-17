using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class EmptyContentException()
    : WgException("Content can not be empty");