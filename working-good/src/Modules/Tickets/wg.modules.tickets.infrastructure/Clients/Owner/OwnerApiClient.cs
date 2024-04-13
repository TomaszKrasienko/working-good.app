using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Owner;

internal sealed class OwnerApiClient(
    IModuleClient moduleClient) : IOwnerApiClient
{
    public Task<OwnerDto> GetOwnerAsync()
        => moduleClient.SendAsync<OwnerDto>("owner/get", null);

    public Task<UserDto> GetUserByIdAsync(UserIdDto dto)
        => moduleClient.SendAsync<UserDto>("owner/user/get", dto);

    public Task<UserDto> GetActiveUserByIdAsync(UserIdDto dto)
        => moduleClient.SendAsync<UserDto>("owner/user/active/get", dto);
}