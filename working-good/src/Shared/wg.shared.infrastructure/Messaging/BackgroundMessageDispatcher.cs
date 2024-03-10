using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using wg.shared.infrastructure.Messaging.Channels;

namespace wg.shared.infrastructure.Messaging;

internal sealed class BackgroundMessageDispatcher(
    ILogger<BackgroundMessageDispatcher> logger,
    IMessageChannel messageChannel) : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Running background dispatcher");
        await foreach (var message in messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
        }
    }
}