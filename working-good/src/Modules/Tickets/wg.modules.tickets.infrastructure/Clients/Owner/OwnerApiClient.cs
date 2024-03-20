using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Owner;

internal sealed class OwnerApiClient(
    IModuleClient moduleClient) : IOwnerApiClient
{
    public async Task<IsUserInGroupDto> IsUserInGroup(UserInGroupDto dto)
        => await moduleClient.SendAsync<IsUserInGroupDto>("owner/user-in-group/is-exists/get", dto);

    public async Task<IsUserExistsDto> IsUserExists(UserIdDto dto)
        => await moduleClient.SendAsync<IsUserExistsDto>("owner/user/is-exists/get", dto);
}