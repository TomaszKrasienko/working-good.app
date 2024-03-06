using wg.shared.abstractions.Exceptions;

namespace wg.shared.abstractions.Kernel.Exceptions;

public sealed class EmptyNameException()
    : WgException("Owner name can not be empty");