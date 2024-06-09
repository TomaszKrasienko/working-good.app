using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using wg.shared.infrastructure.Cache.Configuration.Models;
using wg.shared.infrastructure.Serialization;

namespace wg.modules.notifications.core.Cache;

internal sealed class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly TimeSpan _expiration;
    public CacheService(IDistributedCache distributedCache, TimeSpan expiration)
    {
        _distributedCache = distributedCache;
        _expiration = expiration;
    }
    
    public Task Add(string key, string value)
        => _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value));

    public async Task Add<T>(string key, T value) where T : class
    {
        await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value.ToJson()),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(_expiration)
            });
    }

    public Task<string> Get(string key)
        => _distributedCache.GetStringAsync(key);

    public async Task<T> Get<T>(string key) where T : class
        => (await _distributedCache.GetStringAsync(key))?.ToObject<T>();


}