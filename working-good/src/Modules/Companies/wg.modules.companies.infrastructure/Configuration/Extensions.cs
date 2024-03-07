using Microsoft.Extensions.DependencyInjection;
using wg.modules.companies.infrastructure.DAL.Configuration;

namespace wg.modules.companies.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddDal();
}