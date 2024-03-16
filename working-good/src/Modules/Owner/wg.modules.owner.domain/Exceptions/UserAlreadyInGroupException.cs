using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class UserAlreadyInGroupException(Guid userId, Guid groupId)
    : WgException($"User with ID: {userId} already in group with ID: {groupId}");