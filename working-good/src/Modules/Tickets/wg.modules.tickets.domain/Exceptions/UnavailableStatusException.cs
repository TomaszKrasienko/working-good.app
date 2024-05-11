using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class UnavailableStatusException(string value)
    : WgException($"State: {value} is unavailable");