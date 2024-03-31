using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.modules.tickets.application.Clients.Owner;

public interface IOwnerApiClient
{
    Task<IsUserInGroupDto> IsUserInGroupAsync(UserInGroupDto dto);
    Task<IsUserExistsDto> IsUserExistsAsync(UserIdDto dto);
    Task<UserDto> GetUserByIdAsyncAsync(UserIdDto dto);
}