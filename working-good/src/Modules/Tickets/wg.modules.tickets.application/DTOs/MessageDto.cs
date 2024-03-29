namespace wg.modules.tickets.application.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}