namespace wg.modules.tickets.application.DTOs;

public class ActivityDto
{
    public Guid Id { get; set; }
    public DateTime TimeFrom { get; set; }
    public DateTime? TimeTo { get; set; }
    public TimeSpan? Summary { get; set; }
    public string Note { get; set; }
    public bool IsPaid { get; set; }
    public Guid UserId { get; set; }
}