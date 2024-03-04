using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace wg.shared.infrastructure.Modules.Configuration;

public static class Extensions
{
    internal static IServiceCollection AddModules(this IServiceCollection services)
        => services
            .AddModuleLoad();

    private static IServiceCollection AddModuleLoad(this IServiceCollection services)
    {
        var disabledModules = new List<string>();
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        foreach (var (key, value) in configuration.AsEnumerable())
        {
            if (!key.Contains("module:enable"))
            {
                continue;
            }

            if (!bool.Parse(value))
            {
                disabledModules.Add(key.Split(':')[0]);
            }
        }

        return services;
    }

    public static IHostBuilder ConfigureModules(this IHostBuilder builder)
        => builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            foreach (var settings in GetSettings("*"))
            {
                cfg.AddJsonFile(settings);
            }

            foreach (var settings in GetSettings($"*.{ctx.HostingEnvironment.EnvironmentName}"))
            {
                cfg.AddJsonFile(settings);
            }

            IEnumerable<string> GetSettings(string pattern)
                => Directory.EnumerateFiles(ctx.HostingEnvironment.ContentRootPath, $"module.{pattern}.json",
                    SearchOption.AllDirectories);
        });
    
}