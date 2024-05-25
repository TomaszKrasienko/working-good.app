using wg.modules.notifications.core.Clients.Owner.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.notifications.core.Clients.Owner;

internal sealed class OwnerApiClient(
    IModuleClient moduleClient) : IOwnerApiClient
{
    public Task<UserDto> GetActiveUserAsync(UserIdDto dto)
        => moduleClient.SendAsync<UserDto>("owner/users/active/get", dto);
}