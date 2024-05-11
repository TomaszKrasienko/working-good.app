using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;

public sealed record AssignProjectCommand(Guid Id, Guid ProjectId) : ICommand;