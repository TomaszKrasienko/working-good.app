using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using wg.modules.messages.core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace wg.modules.messages.core.Services;

internal sealed class BackgroundSearcher(
    ILogger<BackgroundSearcher> logger,
    IServiceProvider serviceProvider): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(2));
        while (await periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            await Search(stoppingToken);
        }
    }

    private async Task Search(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting searching message");
        using var scope = serviceProvider.CreateScope();
        var searcher = scope.ServiceProvider.GetRequiredService<IMessageSearcher>();
        await searcher.SearchEmails(cancellationToken);
    }
}