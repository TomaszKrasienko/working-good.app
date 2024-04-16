using Microsoft.Extensions.DependencyInjection;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.infrastructure.DAL.Repositories;
using wg.shared.infrastructure.DAL.Configuration;

namespace wg.modules.tickets.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddContext<TicketsDbContext>()
            .AddScoped<ITicketRepository, TicketRepository>();
}