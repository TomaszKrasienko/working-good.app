using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.infrastructure.Configuration;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Modules.Configuration;

namespace wg.modules.tickets.api;

internal sealed class TicketsModule : IModule
{
    internal const string RoutePath = "tickets-module";
    public string Name { get; } = "Tickets";
    
    public void Register(IServiceCollection services)
    {
        services
            .AddInfrastructure();
    }

    public void Use(WebApplication app)
    {
        app
            .UseModuleRequest()
            .Subscribe<IsTicketAvailableForChangesExistsQuery, TicketExistsDto>(
                "tickets/is-exists/get/available-for-changes",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default));
    }
}