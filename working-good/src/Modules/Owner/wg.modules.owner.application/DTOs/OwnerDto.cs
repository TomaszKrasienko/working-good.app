namespace wg.modules.owner.application.DTOs;

public class OwnerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<UserDto> Users { get; set; }
    public List<GroupDto> Groups { get; set; }
}