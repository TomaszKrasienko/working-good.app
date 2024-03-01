using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class InvalidUserEmailException(string value)
    : WgException($"Value: {value} is not valid email address");