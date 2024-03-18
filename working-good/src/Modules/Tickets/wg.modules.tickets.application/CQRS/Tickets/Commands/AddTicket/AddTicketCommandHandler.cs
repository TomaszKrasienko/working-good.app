using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;

internal sealed class AddTicketCommandHandler : ICommandHandler<AddTicketCommand>
{
    public Task HandleAsync(AddTicketCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}