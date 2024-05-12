using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Owner;

internal sealed class OwnerApiClient(
    IModuleClient moduleClient) : IOwnerApiClient
{
    public Task<OwnerDto> GetOwnerAsync(GetOwnerDto dto)
        => moduleClient.SendAsync<OwnerDto>("owner/get", dto);
    
    public Task<UserDto> GetActiveUserByIdAsync(UserIdDto dto)
        => moduleClient.SendAsync<UserDto>("owner/user/active/get", dto);
    
    public Task<IsGroupMembershipExists> IsMembershipExistsAsync(GetMembershipDto dto)
        => moduleClient.SendAsync<IsGroupMembershipExists>("owner/group/is-membership-exists/get", dto);

    public Task<IsActiveUserExistsDto> IsActiveUserExistsAsync(UserIdDto dto)
        => moduleClient.SendAsync<IsActiveUserExistsDto>("owner/user/is-active-exists/get", dto);
}