using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public class EmailAlreadyInUseException(string email)
    : WgException($"Email: {email} is already in use");