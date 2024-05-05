using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events.External.Handlers;

internal sealed class UserDeactivatedHandler(
    ITicketRepository ticketRepository) : IEventHandler<UserDeactivated>
{
    public async Task HandleAsync(UserDeactivated @event)
    {
        var tickets = await ticketRepository.GetAllForAssignedUser(@event.UserId);
        foreach (var ticket in tickets)
        {
            ticket.RemoveAssignedUser();
            await ticketRepository.UpdateAsync(ticket);
        }
    }
}