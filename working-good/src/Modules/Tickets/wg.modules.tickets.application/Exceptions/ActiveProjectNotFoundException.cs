using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class ActiveProjectNotFoundException(Guid projectId)
    : WgException($"Active project with ID: {projectId} does not exist");