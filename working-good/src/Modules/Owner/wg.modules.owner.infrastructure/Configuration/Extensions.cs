using Microsoft.Extensions.DependencyInjection;
using wg.modules.owner.infrastructure.Auth.Configuration;
using wg.modules.owner.infrastructure.DAL.Configuration;

namespace wg.modules.owner.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddAuth()
            .AddDal();
}