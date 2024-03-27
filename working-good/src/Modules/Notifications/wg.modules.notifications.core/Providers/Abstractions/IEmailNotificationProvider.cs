using wg.modules.notifications.core.Models;

namespace wg.modules.notifications.core.Providers.Abstractions;

public interface IEmailNotificationProvider
{
    EmailNotification GetForNewTicket(string recipient, int ticketNumber, string content, string subject);
}