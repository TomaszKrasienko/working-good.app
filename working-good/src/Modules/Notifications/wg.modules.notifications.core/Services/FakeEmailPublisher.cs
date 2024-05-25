using Microsoft.Extensions.Logging;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Services.Abstractions;

namespace wg.modules.notifications.core.Services;

internal sealed class FakeEmailPublisher(
    ILogger<FakeEmailPublisher> logger) : IEmailPublisher
{
    public Task PublishAsync(EmailNotification emailNotification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending email notification\n"
            + $"Recipient: {emailNotification.Recipient}"
            + $"Subject: {emailNotification.Subject}"
            + $"Content: {emailNotification.Content}");
        return Task.CompletedTask;
    }
}