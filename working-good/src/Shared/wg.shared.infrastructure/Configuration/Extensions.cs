using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.infrastructure.CQRS.Configuration;

namespace wg.shared.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies)
        => services
            .AddCqrs(assemblies);

    public static WebApplication UseInfrastructure(this WebApplication app)
        => app;
}