namespace wg.modules.tickets.application.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public bool IsPriority { get; set; }
    public string State { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public Guid? AssignedEmployee { get; set; }
    public Guid? AssignedUser { get; set; }
    public Guid? ProjectId { get; set; }
}