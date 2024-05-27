using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using wg.shared.infrastructure.Serialization;

namespace wg.modules.notifications.core.Cache;

internal sealed class CacheService(
    IDistributedCache distributedCache) : ICacheService
{
    public async Task Add(string key, string value)
        => await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value));

    public async Task Add<T>(string key, T value)
        => await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value.ToJson()));
    
    public async Task<string> Get(string key)
        => await distributedCache.GetStringAsync(key);

    public async Task<T> Get<T>(string key)
        => (await distributedCache.GetStringAsync(key)).ToObject<T>();


}