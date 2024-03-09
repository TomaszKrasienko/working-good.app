using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.Auth;
using wg.shared.infrastructure.Auth.Configuration.Models;

namespace wg.shared.infrastructure.Auth.Configuration;

internal static class Extensions
{
    private const string SectionName = "Jwt";
    
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions()
            .AddServices();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<JwtOptions>(configuration.GetSection(SectionName));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<IAuthenticator, JwtAuthenticator>();
}