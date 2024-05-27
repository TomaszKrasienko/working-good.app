using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Clients.Owner;
using wg.modules.notifications.core.Clients.Owner.DTO;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class UserSignedUpHandler(
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher,
    IOwnerApiClient ownerApiClient) : IEventHandler<UserSignedUp>
{
    public async Task HandleAsync(UserSignedUp @event)
    {
        var userDto = await ownerApiClient
            .GetUserAsync(new UserIdDto()
            {
                Id = @event.Id
            });

        var emailNotification = emailNotificationProvider
            .GetForNewUser(@event.Email, userDto.FirstName, userDto.LastName, @event.VerificationToken);
        
        await emailPublisher.PublishAsync(emailNotification, default);
    }
}