using wg.modules.notifications.core.Clients.Owner;
using wg.modules.notifications.core.Clients.Owner.DTO;

namespace wg.modules.notifications.core.Cache.Decorators;

internal sealed class OwnerApiClientCacheDecorator(
    IOwnerApiClient apiClient,
    ICacheService cacheService) : IOwnerApiClient
{
    public async Task<UserDto> GetActiveUserAsync(UserIdDto dto)
    {
        var cachedUser = await cacheService.Get<UserDto>(dto.Id.ToString());
        if (cachedUser is not null) return cachedUser;
        var userDto = await apiClient.GetActiveUserAsync(dto);
        await cacheService.Add(dto.Id.ToString(), userDto);
        return userDto;
    }
}