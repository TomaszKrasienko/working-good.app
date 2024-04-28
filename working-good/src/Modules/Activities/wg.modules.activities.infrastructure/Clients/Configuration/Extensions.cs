using Microsoft.Extensions.DependencyInjection;
using wg.modules.activities.application.Clients;
using wg.modules.activities.application.Clients.Tickets;
using wg.modules.activities.infrastructure.Clients.Tickets;

namespace wg.modules.activities.infrastructure.Clients.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddApiClients(this IServiceCollection services)
        => services
            .AddSingleton<ITicketsApiClient, TicketsApiClient>();
}