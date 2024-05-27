using wg.modules.notifications.core.Clients.Owner.DTO;

namespace wg.modules.notifications.core.Clients.Owner;

public interface IOwnerApiClient
{
    Task<UserDto> GetUserAsync(UserIdDto dto);
}