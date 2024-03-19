using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class ProjectDoesNotExists(Guid projectId, Guid employeeId)
    : WgException($"Project with ID: {projectId} does not exist for company with employee with ID: {employeeId}");