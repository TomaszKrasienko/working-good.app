using System.Collections.Immutable;

namespace wg.modules.owner.application.DTOs;

public class OwnerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ImmutableList<UserDto> Users { get; set; }
    public ImmutableList<GroupDto> Groups { get; set; }
}