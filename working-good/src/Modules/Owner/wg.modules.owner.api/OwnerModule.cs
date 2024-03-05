using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.owner.infrastructure.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.owner.api;

internal sealed class OwnerModule : IModule
{
    internal const string RoutePath = "owner-module";
    public string Name { get; } = "Owner";
    
    public void Register(IServiceCollection services)
    {
        services.AddInfrastructure();
    }

    public void Use(WebApplication app)
    { 
        
    }
}