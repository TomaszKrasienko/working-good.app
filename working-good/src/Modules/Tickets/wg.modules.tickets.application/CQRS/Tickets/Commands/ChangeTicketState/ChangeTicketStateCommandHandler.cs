using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;

internal sealed class ChangeTicketStateCommandHandler(
    ITicketRepository ticketRepository) : ICommandHandler<ChangeTicketStateCommand>
{
    public Task HandleAsync(ChangeTicketStateCommand command, CancellationToken cancellationToken)
    {
        throw new Exception();
    }
}