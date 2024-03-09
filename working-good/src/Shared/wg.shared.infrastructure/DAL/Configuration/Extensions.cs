using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.infrastructure.Configuration;
using wg.shared.infrastructure.DAL.Configuration.Models;

namespace wg.shared.infrastructure.DAL.Configuration;

public static class Extensions
{
    private const string SectionName = "DAL";
    
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration);

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<DalOptions>(configuration.GetSection(SectionName));

    public static IServiceCollection AddContext<T>(this IServiceCollection services) where T : DbContext
    {
        var options = services.GetOptions<DalOptions>();
        services.AddDbContext<T>(x => x.UseSqlServer(options.ConnectionString));
        return services;
    }
}