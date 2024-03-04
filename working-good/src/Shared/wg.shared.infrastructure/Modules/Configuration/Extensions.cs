using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using wg.shared.infrastructure.Providers;

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
        var configuredModules = configuration
            .AsEnumerable()
            .Where(x => x.Key.Contains("module:enabled"));
        foreach (var (key, value) in configuredModules)
        {
            if (bool.TryParse(value, out var result) || !result)
            {
                disabledModules.Add(key.Split(':')[0]);
            }
        }
        services.ConfigureControllers(disabledModules);
        return services;
    }

    private static void ConfigureContro(this IServiceCollection services, List<string> disabledModules)
    {
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                var removedParts = new List<ApplicationPart>();
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager
                        .ApplicationParts
                        .Where(x => x.Name.Contains(disabledModule, StringComparison.InvariantCultureIgnoreCase));
                    removedParts.AddRange(parts);
                }
                
                foreach (var part in removedParts)
                {
                    manager.ApplicationParts.Remove(part);
                }
                
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
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