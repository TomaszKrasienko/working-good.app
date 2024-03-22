using Microsoft.Extensions.DependencyInjection;
using wg.modules.tickets.infrastructure.Clients.Configuration;
using wg.modules.tickets.infrastructure.DAL.Configuration;

namespace wg.modules.tickets.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddDal()
            .AddClients();
}