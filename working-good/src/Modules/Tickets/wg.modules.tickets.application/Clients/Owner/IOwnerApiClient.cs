using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.modules.tickets.application.Clients.Owner;

public interface IOwnerApiClient
{
    Task<IsUserInGroupDto> IsUserInGroup(UserInGroupDto dto);
    Task<IsUserExistsDto> IsUserExists(UserIdDto dto);
    Task<UserDto> GetUserByIdAsync(UserIdDto dto);
}