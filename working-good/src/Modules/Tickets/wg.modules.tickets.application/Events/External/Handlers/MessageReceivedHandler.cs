using wg.modules.tickets.domain.Services;
using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events.External.Handlers;

internal sealed class MessageReceivedHandler(
    INewMessageDomainService newMessageDomainService) : IEventHandler<MessageReceived>
{
    public async Task HandleAsync(MessageReceived @event)
    {
        await newMessageDomainService.AddNewMessage(Guid.NewGuid(), @event.Sender, @event.Subject,
            @event.Content, @event.CreatedAt, @event.TicketNumber, null, @event.AssignedEmployee);
    }
}