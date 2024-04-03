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
            var ticket = await ticketRepository.GetByNumberAsync((int)@event.TicketNumber);
            if (ticket is null)
            {
                throw new TicketNumberNotFoundException((int)@event.TicketNumber);
            }
            ticket.AddMessage(Guid.NewGuid(), @event.Sender, @event.Subject,
                @event.Content, @event.CreatedAt);
            await ticketRepository.UpdateAsync(ticket);
            return;
        }

        var number = await ticketRepository.GetMaxNumberAsync();
        var newTicket = Ticket.Create(Guid.NewGuid(), number + 1, @event.Subject,
            @event.Content, @event.CreatedAt, Guid.Empty, State.New(), @event.CreatedAt,
            false, null, @event.AssignedEmployee, null, null,
            @event.Sender);
        await ticketRepository.AddAsync(newTicket);
        var ticketCreated = new TicketCreated(newTicket.Id, newTicket.Number, newTicket.Subject,
            newTicket.Content, newTicket.AssignedUser, newTicket.AssignedEmployee);
        await messageBroker.PublishAsync(ticketCreated);
    }
}