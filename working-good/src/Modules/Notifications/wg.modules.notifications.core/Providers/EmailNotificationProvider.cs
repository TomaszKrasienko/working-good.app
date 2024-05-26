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
            Recipient = [recipient],
            Subject = NotificationsDirectory.GetTicketSubject(ticketNumber, subject),
            Content = NotificationsDirectory.GetNewTicketContent(content)
        };
    }

    public EmailNotification GetForNewUser(string recipient, string firstName, string lastName, string verificationToken)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            return null;
        
        if (string.IsNullOrWhiteSpace(firstName))
            return null;
        
        if (string.IsNullOrWhiteSpace(lastName))
            return null;
        
        if (string.IsNullOrWhiteSpace(verificationToken))
            return null;

        return new EmailNotification()
        {
            Recipient = [recipient],
            Subject = NotificationsDirectory.GetNewUserSubject(),
            Content = NotificationsDirectory.GetNewUserContent(firstName, lastName, verificationToken)
        };
    }

    public EmailNotification GetForNewEmployee(string email)
        => new EmailNotification()
        {
            Recipient = [email],
            Content = $"Your email: {email} now has a permission to sending tickets",
            Subject = "Hello in working-good"
        };

    public EmailNotification GetForAssigning(string recipient, int ticketNumber)
    {
        return new EmailNotification()
        {
            Recipient = [recipient],
            Subject = NotificationsDirectory.GetAssigningSubject(ticketNumber),
            Content = NotificationsDirectory.GetAssigningContent(ticketNumber)
        };
    }
}