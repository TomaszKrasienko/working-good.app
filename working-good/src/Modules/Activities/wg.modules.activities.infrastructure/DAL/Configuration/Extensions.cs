using Microsoft.Extensions.DependencyInjection;
using wg.shared.infrastructure.DAL.Configuration;

namespace wg.modules.activities.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddContext<ActivitiesDbContext>();
}