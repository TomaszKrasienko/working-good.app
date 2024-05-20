using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;

internal sealed class ChangeTicketStatusCommandHandler(
    ITicketRepository ticketRepository, IClock clock) : ICommandHandler<ChangeTicketStatusCommand>
{
    public async Task HandleAsync(ChangeTicketStatusCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.Id);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.Id);
        }
        
        ticket.ChangeStatus(command.Status, clock.Now());
        await ticketRepository.UpdateAsync(ticket);
    }
}