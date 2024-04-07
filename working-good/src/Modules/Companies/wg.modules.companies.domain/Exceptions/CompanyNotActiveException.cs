using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public sealed class CompanyNotActiveException(Guid id) 
    : WgException($"Company with ID: {id} is not active");