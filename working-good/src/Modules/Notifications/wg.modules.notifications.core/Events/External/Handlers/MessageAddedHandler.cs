using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;
using wg.shared.infrastructure.Notifications;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class MessageAddedHandler(
    IEmailPublisher emailPublisher) : IEventHandler<MessageAdded>
{
    public async Task HandleAsync(MessageAdded @event)
    {
        if (string.IsNullOrWhiteSpace(@event.Subject))
            return;
        if (string.IsNullOrWhiteSpace(@event.Content))
            return;
        if (@event.Recipients is null || @event.Recipients.Length == 0)
            return;
        
        var emailNotification = new EmailNotification()
        {
            Content = @event.Content,
            Subject = NotificationsDirectory.GetTicketSubject(@event.TicketNumber, @event.Content),
            Recipient = @event.Recipients.ToList()
        };

        await emailPublisher
            .PublishAsync(emailNotification, default);
    }
}