using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class UserSignedUpHandler(
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher) : IEventHandler<UserSignedUp>
{
    public Task HandleAsync(UserSignedUp @event)
    {
        throw new NotImplementedException();
    }
}