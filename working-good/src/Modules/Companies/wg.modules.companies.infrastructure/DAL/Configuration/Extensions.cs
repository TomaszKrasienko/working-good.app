using Microsoft.Extensions.DependencyInjection;
using wg.modules.companies.domain.Repositories;
using wg.modules.companies.infrastructure.DAL.Repositories;

namespace wg.modules.companies.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddSingleton<ICompanyRepository, InMemoryCompanyRepository>();
}