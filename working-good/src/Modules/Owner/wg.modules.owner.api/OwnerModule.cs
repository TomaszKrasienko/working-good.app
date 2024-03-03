using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.Modules;

namespace wg.modules.owner.api;

internal sealed class OwnerModule : IModule
{
    internal const string RoutePath = "owner";
    public string Name { get; } = "Owner";
    
    public void Register(IServiceCollection services)
    {
        
    }

    public void Use(WebApplication app)
    { 
        
    }
}