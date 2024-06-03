using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.infrastructure.DAL.Configuration;

namespace wg.modules.wiki.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddDal();
}