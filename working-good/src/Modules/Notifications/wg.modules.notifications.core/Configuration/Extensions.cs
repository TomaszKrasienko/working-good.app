using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Cache.Configuration;
using wg.modules.notifications.core.Clients.Configuration;
using wg.modules.notifications.core.Providers.Configuration;
using wg.modules.notifications.core.Services.Configuration;

namespace wg.modules.notifications.core.Configuration;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
        => services
            .AddClients()
            .AddCacheServices()
            .AddServices()
            .AddProviders();
}