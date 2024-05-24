using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using wg.shared.infrastructure.Cache.Configuration.Models;

namespace wg.shared.infrastructure.Cache.Configuration;

internal static class Extensions
{
    private const string SectionName = "Redis";

    internal static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions()
            .AddConnection();

    private static IServiceCollection AddConnection(this IServiceCollection services)
        => services.AddSingleton<IDatabase>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            var cluster = ConnectionMultiplexer.Connect(options.Host);
            return cluster.GetDatabase();
        });

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<RedisOptions>(configuration.GetSection(SectionName));
}