using Microsoft.Extensions.DependencyInjection;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.infrastructure.Clients.Companies;
using wg.modules.tickets.infrastructure.Clients.Owner;

namespace wg.modules.tickets.infrastructure.Clients.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddClients(this IServiceCollection services)
        => services
            .AddSingleton<ICompaniesApiClient, CompaniesApiClient>()
            .AddSingleton<IOwnerApiClient, OwnerApiClient>();
}