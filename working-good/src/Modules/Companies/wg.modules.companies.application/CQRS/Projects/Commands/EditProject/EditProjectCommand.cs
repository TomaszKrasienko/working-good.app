namespace wg.modules.companies.application.CQRS.Projects.Commands.EditProject;

public record EditProjectCommand(Guid Id, string Title, string Description,
    DateTime? PlannedStart = null, DateTime? PlannedFinish = null);