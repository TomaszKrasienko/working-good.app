using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeProject;

public sealed record ChangeProjectCommand(Guid TicketId, Guid ProjectId) : ICommand;