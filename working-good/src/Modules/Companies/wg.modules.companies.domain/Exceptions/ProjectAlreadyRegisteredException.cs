using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public class ProjectAlreadyRegisteredException(string title)
    : WgException($"Project with title: {title} is already registered");