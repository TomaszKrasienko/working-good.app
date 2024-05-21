using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.UpdateTicket;

internal sealed class UpdateTicketCommandHandler(
    ITicketRepository ticketRepository) : ICommandHandler<UpdateTicketCommand>
{
    public async Task HandleAsync(UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository
            .GetByIdAsync(command.Id);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.Id);
        }
        
        ticket.ChangeSubject(command.Subject);
        ticket.ChangeContent(command.Content);
        await ticketRepository.UpdateAsync(ticket);
    }
}