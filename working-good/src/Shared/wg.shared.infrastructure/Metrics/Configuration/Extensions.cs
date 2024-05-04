using App.Metrics;
using Microsoft.Extensions.DependencyInjection;

namespace wg.shared.infrastructure.Metrics.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddMetrics(this IServiceCollection services)
    {
        var metricsBuilder = new MetricsBuilder().Configuration.Configure(cfg =>
        {
            
        });

        var metrics = metricsBuilder.Build();
        
        
        return services;
    }
}