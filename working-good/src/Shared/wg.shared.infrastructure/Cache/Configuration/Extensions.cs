using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using wg.shared.infrastructure.Cache.Configuration.Models;
using wg.shared.infrastructure.Configuration;

namespace wg.shared.infrastructure.Cache.Configuration;

internal static class Extensions
{
    private const string SectionName = "Redis";

    internal static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddRedisConnection(configuration);

    private static IServiceCollection AddRedisConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<RedisOptions>(SectionName);
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = options.Host;
        });
        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<RedisOptions>(configuration.GetSection(SectionName));
}