using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class ActiveUserNotFoundException(Guid userId)
    : WgException($"Active user with ID: {userId} does not exist"); 