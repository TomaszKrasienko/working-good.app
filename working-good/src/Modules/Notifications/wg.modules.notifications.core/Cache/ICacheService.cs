namespace wg.modules.notifications.core.Cache;

public interface ICacheService
{
    Task Add(string key, string value);
    Task Add<T>(string key, T value);
    Task<string> Get(string key);
    Task<T> Get<T>(string key);
}