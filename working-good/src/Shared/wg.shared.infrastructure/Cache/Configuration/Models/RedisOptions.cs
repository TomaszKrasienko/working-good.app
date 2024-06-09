namespace wg.shared.infrastructure.Cache.Configuration.Models;

public sealed record RedisOptions
{
    public string Host { get; init; }
    public TimeSpan Expiration { get; init; }
}