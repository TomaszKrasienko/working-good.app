using Microsoft.Extensions.DependencyInjection;
using wg.modules.tickets.domain.Services;

namespace wg.modules.tickets.domain.Configuration;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services;
}