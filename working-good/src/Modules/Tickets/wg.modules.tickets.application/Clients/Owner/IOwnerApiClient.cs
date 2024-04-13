using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.modules.tickets.application.Clients.Owner;

public interface IOwnerApiClient
{
    Task<OwnerDto> GetOwnerAsync();
    Task<UserDto> GetActiveUserByIdAsync(UserIdDto dto);
}