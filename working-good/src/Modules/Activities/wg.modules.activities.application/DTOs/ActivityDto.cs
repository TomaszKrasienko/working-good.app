namespace wg.modules.activities.application.DTOs;

public class ActivityDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime TimeFrom { get; set; }
    public DateTime? TimeTo { get; set; }
    public TimeSpan Summary { get; set; }
}