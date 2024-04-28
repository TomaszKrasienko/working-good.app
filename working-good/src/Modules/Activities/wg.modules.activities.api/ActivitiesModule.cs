using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.activities.infrastructure.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.activities.api;

internal sealed class ActivitiesModule : IModule
{
    internal const string RoutePath = "activities-module";
    public string Name { get; } = "Activities";
    public void Register(IServiceCollection services)
    {
        services.AddInfrastructure();
    }

    public void Use(WebApplication app)
    {
        
    }
}