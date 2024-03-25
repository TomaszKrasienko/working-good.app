using Microsoft.Extensions.Hosting;
using wg.modules.messages.core.Services.Abstractions;

namespace wg.modules.messages.core.Services;

internal sealed class BackgroundSearcher
    (IMessageSearcher messageSearcher): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await messageSearcher.SearchEmails(stoppingToken);
    }
}