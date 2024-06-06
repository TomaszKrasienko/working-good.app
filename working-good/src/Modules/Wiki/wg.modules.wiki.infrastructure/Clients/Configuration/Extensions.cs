using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.application.Clients.Companies;
using wg.modules.wiki.application.Clients.Tickets;

namespace wg.modules.wiki.infrastructure.Clients.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddClients(this IServiceCollection services)
        => services
            .AddSingleton<ICompaniesApiClient, CompaniesApiClient>()
            .AddSingleton<ITicketsApiClient, TicketsApiClient>();
}