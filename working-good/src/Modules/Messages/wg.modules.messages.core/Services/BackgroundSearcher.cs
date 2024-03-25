using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using wg.modules.messages.core.Services.Abstractions;

namespace wg.modules.messages.core.Services;

internal sealed class BackgroundSearcher
    (ServiceProvider serviceProvider): BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Timer timer = new Timer(async (_) => Search(stoppingToken), null, TimeSpan.Zero, TimeSpan.FromMinutes(3));
        return Task.CompletedTask;
    }

    private async void Search(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var searcher = scope.ServiceProvider.GetRequiredService<IMessageSearcher>();
        await searcher.SearchEmails(cancellationToken);
    }
}