using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Services.Abstractions;

namespace wg.tests.shared.Integration;

internal sealed class FakeEmailPublisher : IEmailPublisher
{
    public Task PublishAsync(EmailNotification emailNotification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}