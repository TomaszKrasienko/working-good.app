using wg.modules.notifications.core.Models;

namespace wg.modules.notifications.core.Services.Abstractions;

public interface IEmailPublisher
{
    Task PublishAsync(EmailNotification emailNotification, CancellationToken cancellationToken);
}