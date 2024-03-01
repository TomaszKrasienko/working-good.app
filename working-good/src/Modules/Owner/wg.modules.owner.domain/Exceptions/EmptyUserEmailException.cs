using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class EmptyUserEmailException()
    : WgException("User email can not be null or empty");