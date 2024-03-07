using wg.modules.companies.domain.ValueObjects.Company;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public class EmailNotMatchToEmailDomainException(string email, EmailDomain emailDomain)
    : WgException($"Email: {email} does not match to {emailDomain}");