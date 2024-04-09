using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.Events.External.Handlers;

internal sealed class EmployeeDeactivatedHandler(
    ITicketRepository ticketRepository,
    IClock clock) : IEventHandler<EmployeeDeactivated>
{
    public async Task HandleAsync(EmployeeDeactivated @event)
    {
        var tickets = await ticketRepository.GetAllForAssignedEmployee(@event.EmployeeId);
        foreach (var ticket in tickets)
        {
            ticket.ChangeAssignedUser(@event.SubstituteEmployeeId, clock.Now());
            await ticketRepository.UpdateAsync(ticket);
        }
    }
}