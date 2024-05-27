using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using wg.shared.infrastructure.Serialization;

namespace wg.modules.notifications.core.Cache;

internal sealed class CacheService(
    IDistributedCache distributedCache) : ICacheService
{
    public Task Add(string key, string value)
        => distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value));

    public Task Add<T>(string key, T value) where T : class
        => distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value.ToJson()));
    
    public Task<string> Get(string key)
        => distributedCache.GetStringAsync(key);

    public async Task<T> Get<T>(string key) where T : class
        => (await distributedCache.GetStringAsync(key))?.ToObject<T>();


}