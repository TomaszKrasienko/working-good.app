using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class TicketCreatedHandler(
    ICompaniesApiClient companiesApiClient,
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher) : IEventHandler<TicketCreated>
{
    public Task HandleAsync(TicketCreated @event)
    {
        throw new NotImplementedException();
    }
}