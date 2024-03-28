using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Clients.Companies;

namespace wg.modules.notifications.core.Clients.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddClients(this IServiceCollection services)
        => services
            .AddSingleton<ICompaniesApiClient, CompaniesApiClient>();
}