using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.activities.application.CQRS.AddActivity;

public sealed record AddActivityCommand(Guid Id, Guid TicketId, string Content, DateTime TimeFrom, DateTime? TimeTo) : ICommand;