using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Services.Abstractions;

namespace wg.modules.notifications.core.Services;

internal sealed class EmailPublisher : IEmailPublisher
{
    public Task PublishAsync(EmailNotification emailNotification)
    {
        throw new NotImplementedException();
    }
}