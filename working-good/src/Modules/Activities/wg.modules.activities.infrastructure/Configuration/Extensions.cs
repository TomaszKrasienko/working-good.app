using Microsoft.Extensions.DependencyInjection;
using wg.modules.activities.infrastructure.Clients.Configuration;
using wg.modules.activities.infrastructure.DAL.Configuration;

namespace wg.modules.activities.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddDal()
            .AddApiClients();
}