using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class InvalidProjectForEmployeeException(Guid employeeId, Guid projectId)
    : WgException($"Project with ID: {projectId} is not assigned for company of employee with ID: {employeeId}");