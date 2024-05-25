using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class UserSignedUpHandler(
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher,
    ICacheService cacheService) : IEventHandler<UserSignedUp>
{
    public async Task HandleAsync(UserSignedUp @event)
    {
        await cacheService.Add(@event.Id.ToString(), @event.Email);
        
        var emailNotification = emailNotificationProvider
            .GetForNewUser(@event.Email, @event.FirstName, @event.LastName, @event.VerificationToken);
        
        if (emailNotification is null)
            return;
        
        await emailPublisher.PublishAsync(emailNotification, default);
    }
}