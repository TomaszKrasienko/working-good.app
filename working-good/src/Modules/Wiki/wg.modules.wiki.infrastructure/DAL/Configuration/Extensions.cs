using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.domain.Repositories;
using wg.modules.wiki.infrastructure.DAL.Repositories.Internals;
using wg.shared.infrastructure.DAL.Configuration;

namespace wg.modules.wiki.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddContext<WikiDbContext>()
            .AddScoped<ISectionRepository, SqlServerSectionRepository>();
}