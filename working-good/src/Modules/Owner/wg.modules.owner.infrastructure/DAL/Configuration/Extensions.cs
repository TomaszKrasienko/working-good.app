using Microsoft.Extensions.DependencyInjection;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.infrastructure.DAL.Repositories;

namespace wg.modules.owner.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddSingleton<IOwnerRepository, InMemoryOwnerRepository>();
}