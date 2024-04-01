using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class UserSignedUpHandler(
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher) : IEventHandler<UserSignedUp>
{
    public async Task HandleAsync(UserSignedUp @event)
    {
        var emailNotification = emailNotificationProvider
            .GetForNewUser(@event.Email, @event.FirstName, @event.LastName, @event.VerificationToken);
        
        if (emailNotification is null)
            return;
        
        await emailPublisher.PublishAsync(emailNotification, default);
    }
}