using Microsoft.Extensions.DependencyInjection;

namespace wg.shared.infrastructure.Metrics.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddMetrics(this IServiceCollection services)
    {
        return services;
    }
}