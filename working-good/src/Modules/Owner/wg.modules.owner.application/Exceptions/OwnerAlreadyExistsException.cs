using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.application.Exceptions;

public sealed class OwnerAlreadyExistsException() 
    : WgException("Owner already exists");