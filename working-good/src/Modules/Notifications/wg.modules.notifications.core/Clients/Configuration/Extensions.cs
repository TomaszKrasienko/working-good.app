using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Owner;

namespace wg.modules.notifications.core.Clients.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddClients(this IServiceCollection services)
        => services
            .AddSingleton<ICompaniesApiClient, CompaniesApiClient>()
            .AddSingleton<IOwnerApiClient, OwnerApiClient>();
}