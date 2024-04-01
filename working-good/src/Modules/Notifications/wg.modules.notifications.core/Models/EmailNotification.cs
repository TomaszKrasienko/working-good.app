namespace wg.modules.notifications.core.Models;

public class EmailNotification
{
    public List<string> Recipient { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
}