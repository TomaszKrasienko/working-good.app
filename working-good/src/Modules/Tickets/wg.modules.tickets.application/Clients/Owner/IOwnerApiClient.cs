using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.modules.tickets.application.Clients.Owner;

public interface IOwnerApiClient
{
    Task<bool> IsUserInGroup(UserInGroupDto dto);
    Task<bool> IsUserExists(Guid id);
}