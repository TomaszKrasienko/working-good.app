namespace wg.modules.owner.application.DTOs;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<Guid> Users { get; set; }
}