namespace wg.modules.tickets.application.Clients.Companies.DTO;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? PlannedStart { get; set; }
    public DateTime? PlannedFinish { get; set; }
}