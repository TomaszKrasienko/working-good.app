using App.Metrics;
using App.Metrics.Counter;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.shared.infrastructure.Metrics.Decorators;

internal sealed class CommandMetricsDecorator<T>(
    ICommandHandler<T> handler, IServiceScopeFactory serviceScopeFactory) : ICommandHandler<T> where T : ICommand
{
    public async Task HandleAsync(T command, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var metricsRoot = scope.ServiceProvider.GetRequiredService<IMetricsRoot>();
        metricsRoot.Measure.Counter.Increment(new CounterOptions()
        {
            Name = "command",
            Tags = new MetricTags("command", command.GetType().Name)
        });

        await handler.HandleAsync(command, cancellationToken);
    }
}