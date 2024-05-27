using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Clients.Owner;
using wg.modules.notifications.core.Clients.Owner.DTO;

namespace wg.modules.notifications.core.Cache.Decorators;

internal sealed class OwnerApiClientCacheDecorator(
    IOwnerApiClient apiClient,
    IServiceProvider servicesProvider) : IOwnerApiClient
{
    public async Task<UserDto> GetUserAsync(UserIdDto dto)
    {
        using var scope = servicesProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
        var cachedUser = await cacheService.Get<UserDto>(dto.Id.ToString());
        if (cachedUser is not null) return cachedUser;
        var userDto = await apiClient.GetUserAsync(dto);
        await cacheService.Add(dto.Id.ToString(), userDto);
        return userDto;
    }
}