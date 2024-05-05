using System.Reflection;
using System.Runtime.Serialization;
using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using wg.shared.infrastructure.Auth.Configuration;
using wg.shared.infrastructure.Context.Configuration;
using wg.shared.infrastructure.CQRS.Configuration;
using wg.shared.infrastructure.DAL.Configuration;
using wg.shared.infrastructure.Events.Configuration;
using wg.shared.infrastructure.Exceptions.Configuration;
using wg.shared.infrastructure.Logging.Configuration;
using wg.shared.infrastructure.Mailbox.Configuration;
using wg.shared.infrastructure.Messaging.Configuration;
using wg.shared.infrastructure.Metrics.Configuration;
using wg.shared.infrastructure.Modules.Configuration;
using wg.shared.infrastructure.Time.Configuration;
using wg.shared.infrastructure.Vault.Configuration;

namespace wg.shared.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies,
        IConfiguration configuration)
        => services
            .AddModules(assemblies)
            .AddCqrs(assemblies)
            .AddTime()
            .AddAuth(configuration)
            .AddDal(configuration)
            .AddExceptionMiddleware()
            .AddMessaging()
            .AddEvents(assemblies)
            .AddIdentityContext()
            .AddMailbox(configuration)
            .AddAppMetrics()
            .AddLogging(assemblies)
            .AddUiDocumentation()
            .AddBanner();

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

    private static IServiceCollection AddBanner(this IServiceCollection services)
    {
        Console.WriteLine(FiggleFonts.Doom.Render("working-good"));
        return services;
    }

    internal static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetService<IOptions<T>>().Value;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
        => app
            .UseControllers()
            .UseUiDocumentation()
            .UseAppMetrics()
            .UseExceptionMiddleware()
            .UseAuth();

    public static WebApplicationBuilder UseInfrastructure(this WebApplicationBuilder app)
        => app.UseSerilog();
    
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

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        T t = new T();
        configuration.Bind(sectionName, t);
        return t;
    }
}