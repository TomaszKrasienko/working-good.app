using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class UserDoesNotBelongToGroupException(Guid projectId, Guid userId)
    : WgException($"User with ID: {userId} does not belong to group with ID: {projectId}");