using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class TicketCreatedHandler(
    ICompaniesApiClient companiesApiClient,
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher) : IEventHandler<TicketCreated>
{
    public async Task HandleAsync(TicketCreated @event)
    {
        if (@event.EmployeeId is null)
        {
            return;
        }

        var recipientEmail = await companiesApiClient.GetEmployeeEmail(new EmployeeIdDto()
        {
            Value = (Guid)@event.EmployeeId
        });
        var notification = emailNotificationProvider.GetForNewTicket(recipientEmail.Value,
            @event.TicketNumber, @event.Content, @event.Subject);
        if (notification is null)
        {
            return;
        }
        await emailPublisher.PublishAsync(notification, default);
    }
}