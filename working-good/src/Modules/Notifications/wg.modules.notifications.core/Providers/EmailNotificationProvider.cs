using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.shared.infrastructure.Notifications;

namespace wg.modules.notifications.core.Providers;

internal sealed class EmailNotificationProvider : IEmailNotificationProvider
{
    public EmailNotification GetForNewTicket(string recipient, int ticketNumber, string content, string subject)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            return null;

        if (ticketNumber <= 0)
            return null;

        if (string.IsNullOrWhiteSpace(content))
            return null;

        if (string.IsNullOrWhiteSpace(subject))
            return null;

        return new EmailNotification()
        {
            Recipient = recipient,
            Subject = NotificationsDirectory.GetNewTicketSubject(ticketNumber, subject),
            Content = NotificationsDirectory.GetNewTicketContent(content)
        };
    }
}