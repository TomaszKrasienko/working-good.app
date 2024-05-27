using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Cache.Decorators;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Owner;

namespace wg.modules.notifications.core.Cache.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddCacheServices(this IServiceCollection services)
        => services
            .AddServices()
            .AddDecorators();

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<ICacheService, CacheService>();

    private static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services.Decorate<IOwnerApiClient,OwnerApiClientCacheDecorator>();
        services.Decorate<ICompaniesApiClient, CompaniesApiClientCacheDecorator>();
        return services;
    }
}