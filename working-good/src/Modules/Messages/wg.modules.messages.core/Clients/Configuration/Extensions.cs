using Microsoft.Extensions.DependencyInjection;
using wg.modules.messages.core.Clients.Companies;

namespace wg.modules.messages.core.Clients.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddClients(this IServiceCollection services)
        => services
            .AddSingleton<ICompaniesApiClient, CompaniesApiClient>();
}