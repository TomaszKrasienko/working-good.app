using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketStatus;

internal sealed class ChangeTicketStatusCommandHandler(
    ITicketRepository ticketRepository) : ICommandHandler<ChangeTicketStatusCommand>
{
    public Task HandleAsync(ChangeTicketStatusCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}