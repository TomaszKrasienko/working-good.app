using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using wg.shared.infrastructure.Cache.Configuration.Models;
using wg.shared.infrastructure.Serialization;

namespace wg.modules.notifications.core.Cache;

internal sealed class CacheService(
    IDistributedCache distributedCache, TimeSpan expiration) : ICacheService
{
    public async Task Add<T>(string key, T value) where T : class
    {
        await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value.ToJson()),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(expiration)
            });
    }

    public async Task<T> Get<T>(string key) where T : class
        => (await distributedCache.GetStringAsync(key))?.ToObject<T>();
}