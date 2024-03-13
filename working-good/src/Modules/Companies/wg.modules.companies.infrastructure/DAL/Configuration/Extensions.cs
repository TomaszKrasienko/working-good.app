using Microsoft.Extensions.DependencyInjection;
using wg.modules.companies.domain.Repositories;
using wg.modules.companies.infrastructure.DAL.Repositories;
using wg.shared.infrastructure.DAL.Configuration;

namespace wg.modules.companies.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddContext<CompaniesDbContext>()
            .AddScoped<ICompanyRepository, SqlServerCompanyRepository>();
}