using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Providers.Abstractions;

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
            Subject = $"Ticket number #{ticketNumber} - {subject}",
            Content = $"A ticket has been created to which you have been assigned with the following content\n{content}"
        };
    }
}