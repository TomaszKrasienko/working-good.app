using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Activities.Commands.ChangeActivityType;

internal sealed class ChangeActivityTypeCommandHandler(
    ITicketRepository ticketRepository) : ICommandHandler<ChangeActivityTypeCommand>
{
    public async Task HandleAsync(ChangeActivityTypeCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByActivityId(command.Id);
        if (ticket is null)
        {
            throw new ActivityNotFoundException(command.Id);
        }
        
        ticket.ChangeActivityType(command.Id);
        await ticketRepository.UpdateAsync(ticket);
    }
}