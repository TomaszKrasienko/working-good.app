using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.application.Exceptions;

public sealed class OwnerNotFoundException() 
    : WgException($"Owner not found");