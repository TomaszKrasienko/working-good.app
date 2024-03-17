using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class NumberDoesNotExistsException(int number) 
    : WgException($"Number: {number} is not registered");