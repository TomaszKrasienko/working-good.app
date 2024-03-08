using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using wg.shared.infrastructure.Auth.Configuration;
using wg.shared.infrastructure.CQRS.Configuration;
using wg.shared.infrastructure.Modules.Configuration;
using wg.shared.infrastructure.Time.Configuration;

namespace wg.shared.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies,
        IConfiguration configuration)
        => services
            .AddModules()
            .AddCqrs(assemblies)
            .AddTime()
            .AddUiDocumentation()
            .AddAuth(configuration);

    private static IServiceCollection AddUiDocumentation(this IServiceCollection services)
        => services.AddSwaggerGen(swagger =>
        {
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "working-good",
                Version = "v1"
            });
        });

    internal static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        using var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetService<IOptions<T>>().Value;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
        => app
            .UseControllers()
            .UseUiDocumentation();

    private static WebApplication UseUiDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(swagger =>
        {
            swagger.RoutePrefix = "swagger";
            swagger.DocumentTitle = "working-good.API";
        });
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = "redoc";
            reDoc.SpecUrl("/swagger/v1/swagger.json");
            reDoc.DocumentTitle = "working-good.API";
        });
        return app;
    }

    private static WebApplication UseControllers(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}