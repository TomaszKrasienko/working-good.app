using wg.shared.abstractions.Exceptions;

namespace wg.shared.abstractions.Kernel.Exceptions;

public sealed class InvalidEmailException(string value)
    : WgException($"Value: {value} is not valid email address");