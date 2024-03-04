using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.infrastructure.CQRS.Configuration;
using wg.shared.infrastructure.Modules.Configuration;

namespace wg.shared.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies)
        => services
            .AddModules()
            .AddCqrs(assemblies);

    public static WebApplication UseInfrastructure(this WebApplication app)
        => app;
}