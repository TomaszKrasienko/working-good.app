using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.tickets.domain.Configuration;
using wg.modules.tickets.infrastructure.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.api;

internal sealed class TicketsModule : IModule
{
    internal const string RoutePath = "tickets-module";
    public string Name { get; } = "Tickets";
    
    public void Register(IServiceCollection services)
    {
        services
            .AddInfrastructure()
            .AddDomain();
    }

    public void Use(WebApplication app)
    {
        
    }
}