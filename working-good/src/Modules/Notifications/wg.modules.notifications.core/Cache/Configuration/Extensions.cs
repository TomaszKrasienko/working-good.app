using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wg.modules.notifications.core.Cache.Decorators;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Owner;
using wg.shared.infrastructure.Cache.Configuration.Models;

namespace wg.modules.notifications.core.Cache.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddCacheServices(this IServiceCollection services)
        => services
            .AddServices()
            .AddDecorators();

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<ICacheService>(sp =>
            {
                var distributedCache = sp.GetRequiredService<IDistributedCache>();
                var options = sp.GetRequiredService<IOptions<RedisOptions>>();
                return new CacheService(distributedCache, options.Value.Expiration);
            });

    private static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services.Decorate<IOwnerApiClient,OwnerApiClientCacheDecorator>();
        services.Decorate<ICompaniesApiClient, CompaniesApiClientCacheDecorator>();
        return services;
    }
}