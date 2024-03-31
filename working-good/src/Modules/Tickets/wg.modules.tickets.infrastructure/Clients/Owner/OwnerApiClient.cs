using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Owner;

internal sealed class OwnerApiClient(
    IModuleClient moduleClient) : IOwnerApiClient
{
    public Task<IsUserInGroupDto> IsUserInGroupAsync(UserInGroupDto dto)
        => moduleClient.SendAsync<IsUserInGroupDto>("owner/user-in-group/is-exists/get", dto);

    public Task<IsUserExistsDto> IsUserExistsAsync(UserIdDto dto)
        => moduleClient.SendAsync<IsUserExistsDto>("owner/user/is-exists/get", dto);

    public Task<UserDto> GetUserByIdAsyncAsync(UserIdDto dto)
        => moduleClient.SendAsync<UserDto>("owner/user/get", dto);
}