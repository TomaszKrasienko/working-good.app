using wg.modules.tickets.application.Events.Mappers;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Messaging;

namespace wg.modules.tickets.application.Events.External.Handlers;

internal sealed class MessageReceivedHandler(
    ITicketRepository ticketRepository,
    IMessageBroker messageBroker) : IEventHandler<MessageReceived>
{
    public async Task HandleAsync(MessageReceived @event)
    {
        if (@event.TicketNumber is not null)
        {
            var ticket = await ticketRepository.GetByNumberAsync(@event.TicketNumber.Value);
            if (ticket is null)
            {
                throw new TicketNumberNotFoundException(@event.TicketNumber.Value);
            }
            ticket.AddMessage(Guid.NewGuid(), @event.Sender, @event.Subject,
                @event.Content, @event.CreatedAt, false);
            await ticketRepository.UpdateAsync(ticket); 
            return;
        }
        
        var number = await ticketRepository.GetMaxNumberAsync();
        var newTicket = Ticket.Create(Guid.NewGuid(), number + 1, @event.Subject,
            @event.Content, @event.CreatedAt, @event.Sender);
        newTicket.ChangeAssignedEmployee(@event.AssignedEmployee);
        await ticketRepository.AddAsync(newTicket);
        await messageBroker.PublishAsync(newTicket.AsEvent());
    }
}