using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public sealed class EmptyEmailDomainException() 
    : WgException("Email domain can not be empty");