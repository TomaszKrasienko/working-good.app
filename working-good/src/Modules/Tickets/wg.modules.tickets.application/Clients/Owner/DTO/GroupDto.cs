namespace wg.modules.tickets.application.Clients.Owner.DTO;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<Guid> Users { get; set; }
}