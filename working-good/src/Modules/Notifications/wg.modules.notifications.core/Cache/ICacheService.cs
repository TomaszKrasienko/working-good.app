namespace wg.modules.notifications.core.Cache;

public interface ICacheService
{
    Task Add(string key, string value);
    Task<string> Get(string key);
}