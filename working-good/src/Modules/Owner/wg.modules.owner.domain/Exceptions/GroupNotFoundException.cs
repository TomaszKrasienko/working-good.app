using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class GroupNotFoundException(Guid id)
    : WgException($"Group with ID: {id} does not exist");