using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace wg.shared.infrastructure.Metrics.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddAppMetrics(this IServiceCollection services)
    {
        services.Configure<KestrelServerOptions>(o => o.AllowSynchronousIO = true);
        services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);
        
        var metricsBuilder = new MetricsBuilder().Configuration.Configure(cfg =>
        {
            cfg.AddAppTag("working-good");
            cfg.AddEnvTag("development");
            cfg.AddServerTag("local");
        });

        var metricsWebHostOptions = new MetricsWebHostOptions();
        metricsWebHostOptions.EndpointOptions = options =>
        {
            options.MetricsEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        };

        services.AddMetricsTrackingMiddleware();
        services.AddMetricsEndpoints();
        
        var metrics = metricsBuilder.Build();
        services.AddMetrics(metrics);
        services.AddMetricsEndpoints();
        services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
        services.AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter());
        services.AddMetricsReportingHostedService(metricsWebHostOptions.UnobservedTaskExceptionHandler);
        services.AddMetricsEndpoints(metricsWebHostOptions.EndpointOptions);
        return services;
    }

    internal static WebApplication UseAppMetrics(this WebApplication app)
    {
        app
            .UseMetricsAllEndpoints()
            .UseMetricsAllMiddleware();

        return app;
    }
}