using wg.modules.notifications.core.Models;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.notifications.core.Providers.Abstractions;

public interface IEmailNotificationProvider
{
    EmailNotification GetForNewTicket(string recipient, int ticketNumber, string content, string subject);
    EmailNotification GetForNewUser(string recipient, string firstName, string lastName, string verificationToken);
}