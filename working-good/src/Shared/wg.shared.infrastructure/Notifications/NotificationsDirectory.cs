namespace wg.shared.infrastructure.Notifications;

public static class NotificationsDirectory
{
    public static string GetNewTicketSubject(int ticketNumber, string subject)
        => $"Ticket number #{ticketNumber} - {subject}";

    public static string GetNewTicketPattern()
        => @"Ticket number #(\d+) - ";

    public static string GetNewTicketContent(string content)
        => $"A ticket has been created to which you have been assigned with the following content\n{content}";
}