namespace wg.modules.notifications.core.Cache;

public interface ICacheService
{
    Task Add<T>(string key, T value) where T : class;
    Task<T> Get<T>(string key) where T : class;
}