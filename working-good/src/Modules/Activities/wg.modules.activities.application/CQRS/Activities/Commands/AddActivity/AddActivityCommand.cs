using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.activities.application.CQRS.Activities.Commands.AddActivity;

public sealed record AddActivityCommand(Guid Id, Guid UserId, Guid TicketId, string Content, 
    DateTime TimeFrom, DateTime? TimeTo, bool IsPaid) : ICommand;