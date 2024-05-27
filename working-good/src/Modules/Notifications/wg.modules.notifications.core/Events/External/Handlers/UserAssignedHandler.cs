using wg.modules.notifications.core.Clients.Owner;
using wg.modules.notifications.core.Clients.Owner.DTO;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class UserAssignedHandler(
    IOwnerApiClient ownerApiClient,
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher) : IEventHandler<UserAssigned>
{
    public async Task HandleAsync(UserAssigned @event)
    {
        var userDto = await ownerApiClient.GetUserAsync(new UserIdDto(){ Id = @event.UserId});
        
        var notification = emailNotificationProvider.GetForAssigning(userDto.Email, @event.TicketNumber);
        await emailPublisher.PublishAsync(notification, default);
    }
}