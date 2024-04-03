using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;
using wg.shared.infrastructure.Notifications;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class MessageAddedHandler(
    ICompaniesApiClient companiesApiClient,
    IEmailPublisher emailPublisher) : IEventHandler<MessageAdded>
{
    public async Task HandleAsync(MessageAdded @event)
    {
        if (@event.EmployeeId is null)
            return;
        if (string.IsNullOrWhiteSpace(@event.Subject))
            return;
        if (string.IsNullOrWhiteSpace(@event.Content))
            return;
        
        var recipient = await companiesApiClient.GetEmployeeByIdAsync(new EmployeeIdDto()
        {
            Id = (Guid)@event.EmployeeId
        });
        
        var emailNotification = new EmailNotification()
        {
            Content = @event.Content,
            Subject = NotificationsDirectory.GetTicketSubject(@event.TicketNumber, @event.Content),
            Recipient = [recipient.Email]
        };

        await emailPublisher
            .PublishAsync(emailNotification, default);
    }
}