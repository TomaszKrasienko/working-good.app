using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class SubstituteUserNotFoundException(Guid userId) 
    : WgException($"User for substitute with ID: {userId} does not exists");