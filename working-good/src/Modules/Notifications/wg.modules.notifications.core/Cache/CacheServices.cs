using StackExchange.Redis;

namespace wg.modules.notifications.core.Cache;

internal sealed class CacheService(IDatabase database) : ICacheService
{
    public async Task Add(string key, string value)
        => await database.StringSetAsync(key, value);

    public async Task<string> Get(string key)
        => await database.StringGetAsync(key);
}