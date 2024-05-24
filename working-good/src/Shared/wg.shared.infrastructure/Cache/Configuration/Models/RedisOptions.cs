namespace wg.shared.infrastructure.Cache.Configuration.Models;

internal sealed record RedisOptions
{
    public string Host { get; init; }
}