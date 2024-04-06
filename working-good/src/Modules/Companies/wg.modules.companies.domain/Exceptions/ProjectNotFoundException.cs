using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public sealed class ProjectNotFoundException(Guid id) 
    : WgException($"Project with ID: {id} does not exists");