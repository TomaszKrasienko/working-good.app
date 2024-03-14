using wg.shared.abstractions.Exceptions;

namespace wg.shared.abstractions.Kernel.Exceptions;

public sealed class EmptyTitleException() 
    : WgException("Title can not be empty");