using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.application.Exceptions;

public sealed class OwnerNotFoundException(Guid id) 
    : WgException($"Owner with ID: {id} not found");