using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Projects.Commands.AddProject;

public sealed record AddProjectCommand(Guid CompanyId, Guid Id, string Title, string Description,
    DateTime? PlannedStart, DateTime? PlannedFinish) : ICommand;