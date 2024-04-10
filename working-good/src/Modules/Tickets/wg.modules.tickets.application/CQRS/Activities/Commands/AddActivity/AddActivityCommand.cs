using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Activities.Commands.AddActivity;

public sealed record AddActivityCommand(Guid Id, Guid TicketId, DateTime TimeFrom, DateTime? TimeTo, 
    string Note, bool IsPaid, Guid UserId) : ICommand;