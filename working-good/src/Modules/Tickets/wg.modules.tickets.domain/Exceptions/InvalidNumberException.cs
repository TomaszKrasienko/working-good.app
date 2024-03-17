using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class InvalidNumberException(int value)
    : WgException($"Value: {value} can not be below 1");