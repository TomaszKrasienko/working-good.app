using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.application.Exceptions;

internal class CompanyNameAlreadyInUseException(string name)
    : WgException($"Company name: {name} already in use");