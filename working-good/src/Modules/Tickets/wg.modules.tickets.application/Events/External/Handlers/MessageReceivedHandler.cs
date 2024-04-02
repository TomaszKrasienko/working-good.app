using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events.External.Handlers;

internal sealed class MessageReceivedHandler() : IEventHandler<MessageReceived>
{
    public async Task HandleAsync(MessageReceived @event)
    {
    }
}