using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.core.DAL.Repositories.Abstractions;
using wg.shared.infrastructure.DAL.Configuration;

namespace wg.modules.wiki.core.DAL.Repositories.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddContext<WikiDbContext>()
            .AddScoped<ISectionRepository>();
}