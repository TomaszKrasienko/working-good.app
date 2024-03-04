using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using wg.shared.infrastructure.CQRS.Configuration;
using wg.shared.infrastructure.Modules.Configuration;
using wg.shared.infrastructure.Time.Configuration;

namespace wg.shared.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies)
        => services
            .AddModules()
            .AddCqrs(assemblies)
            .AddTime()
            .AddUiDocumentation();

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

    public static WebApplication UseInfrastructure(this WebApplication app)
        => app
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
}