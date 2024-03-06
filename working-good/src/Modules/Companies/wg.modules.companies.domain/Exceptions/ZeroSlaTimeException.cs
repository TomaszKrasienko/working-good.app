using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.ValueObjects.Company;

public sealed class ZeroSlaTimeException() 
    : WgException("Sla time can not be zero");