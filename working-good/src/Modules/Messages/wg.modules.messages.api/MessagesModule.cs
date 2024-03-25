using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.messages.core.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.messages.api;

internal sealed class MessagesModule : IModule
{
    internal const string RoutePath = "messages-module";
    public string Name { get; } = "Messages";
    
    public void Register(IServiceCollection services)
    {
        services.AddCore();
    }

    public void Use(WebApplication app)
    {
        
    }
}