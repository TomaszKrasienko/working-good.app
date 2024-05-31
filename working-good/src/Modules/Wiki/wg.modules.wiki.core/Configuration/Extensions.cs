using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.core.DAL.Configuration;
using wg.modules.wiki.core.Services.Configuration;

namespace wg.modules.wiki.core.Configuration;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
        => services
            .AddDal()
            .AddServices();
}