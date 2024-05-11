using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.modules.tickets.application.Clients.Owner;

public interface IOwnerApiClient
{
    Task<OwnerDto> GetOwnerAsync(GetOwnerDto dto);
    Task<UserDto> GetActiveUserByIdAsync(UserIdDto dto);
    Task<IsGroupMembershipExists> IsMembershipExistsAsync(GetMembershipDto dto);
}