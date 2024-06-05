using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.application.Exceptions;

public sealed class OriginIdIsInvalidException(string value)
    : WgException($"Value: {value} is invalid as a OriginId");