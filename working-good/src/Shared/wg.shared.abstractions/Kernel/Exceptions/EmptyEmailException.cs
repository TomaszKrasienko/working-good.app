using wg.shared.abstractions.Exceptions;

namespace wg.shared.abstractions.Kernel.Exceptions;

public sealed class EmptyEmailException()
    : WgException("User email can not be null or empty");