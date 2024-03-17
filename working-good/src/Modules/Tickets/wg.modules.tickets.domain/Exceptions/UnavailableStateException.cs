using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class UnavailableStateException(string value)
    : WgException($"State: {value} is unavailable");