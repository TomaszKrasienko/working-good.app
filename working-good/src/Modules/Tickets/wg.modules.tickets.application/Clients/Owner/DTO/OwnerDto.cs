using System.Collections.Immutable;

namespace wg.modules.tickets.application.Clients.Owner.DTO;

public class OwnerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ImmutableList<UserDto> Users { get; set; }
    public ImmutableList<GroupDto> Groups { get; set; }
}