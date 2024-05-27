using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Clients.Owner;
using wg.modules.notifications.core.Clients.Owner.DTO;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class UserAssignedHandler(
    ICacheService cacheService,
    IOwnerApiClient ownerApiClient,
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher) : IEventHandler<UserAssigned>
{
    public async Task HandleAsync(UserAssigned @event)
    {
        var email = await cacheService.Get(@event.UserId.ToString());
        if (string.IsNullOrWhiteSpace(email))
        {
            var userDto = await ownerApiClient.GetUserAsync(new UserIdDto(){ Id = @event.UserId});
            email = userDto.Email;
            await cacheService.Add(userDto.Id.ToString(), userDto.Email);
        }
        
        var notification = emailNotificationProvider.GetForAssigning(email, @event.TicketNumber);
        await emailPublisher.PublishAsync(notification, default);
    }
}