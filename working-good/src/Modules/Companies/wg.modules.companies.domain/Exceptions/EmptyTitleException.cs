using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public sealed class EmptyTitleException() 
    : WgException("Title can not be empty");