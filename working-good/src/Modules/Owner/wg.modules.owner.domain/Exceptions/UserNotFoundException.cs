using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class UserNotFoundException(Guid id)
    : WgException($"User with ID: {id} does not exist");