using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Activities.Commands.AddActivity;

internal sealed class AddActivityCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient) : ICommandHandler<AddActivityCommand>
{
    public async Task HandleAsync(AddActivityCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        var user = await ownerApiClient.GetActiveUserByIdAsync(new UserIdDto(command.UserId));
        if (user is null)
        {
            throw new ActiveUserNotFoundException(command.UserId);
        }
        
        ticket.AddActivity(command.Id, command.TimeFrom, command.TimeTo, command.Note,
                command.IsPaid, command.UserId);

        await ticketRepository.UpdateAsync(ticket);
    }
}