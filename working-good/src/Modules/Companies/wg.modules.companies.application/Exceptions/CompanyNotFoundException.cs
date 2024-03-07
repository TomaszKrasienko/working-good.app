using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.application.Exceptions;

public sealed class CompanyNotFoundException(Guid companyId) 
    : WgException($"Company with Id: {companyId} not found");