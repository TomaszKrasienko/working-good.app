using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.application.Exceptions;

internal class EmailDomainAlreadyInUseException(string emailDomain)
    : WgException($"Email domain: {emailDomain} already in use");