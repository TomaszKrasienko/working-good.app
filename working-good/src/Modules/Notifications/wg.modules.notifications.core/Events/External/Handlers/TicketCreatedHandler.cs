using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class TicketCreatedHandler(
    IEmailPublisher emailPublisher) : IEventHandler<TicketCreated>
{
    public Task HandleAsync(TicketCreated @event)
    {
        throw new NotImplementedException();
    }
}