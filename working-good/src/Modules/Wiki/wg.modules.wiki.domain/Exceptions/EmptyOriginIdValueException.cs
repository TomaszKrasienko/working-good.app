using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.domain.Exceptions;

public sealed class EmptyOriginIdValueException()
    : WgException("Origin Id can not be empty");