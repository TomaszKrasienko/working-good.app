using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class UserNotFoundException(Guid userId) 
    : WgException($"User with ID: {userId} does not exist");