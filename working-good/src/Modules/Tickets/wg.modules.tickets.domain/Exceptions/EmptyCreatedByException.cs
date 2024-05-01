using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class EmptyCreatedByException()
    : WgException("Created by can not be empty");