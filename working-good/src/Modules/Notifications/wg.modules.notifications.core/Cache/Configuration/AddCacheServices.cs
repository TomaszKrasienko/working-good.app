using Microsoft.Extensions.DependencyInjection;

namespace wg.modules.notifications.core.Cache.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddCacheServices(this IServiceCollection services)
        => services.AddScoped<ICacheService, CacheService>();
}