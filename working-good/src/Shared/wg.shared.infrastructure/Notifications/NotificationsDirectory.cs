namespace wg.shared.infrastructure.Notifications;

public static class NotificationsDirectory
{
    public static string GetTicketSubject(int ticketNumber, string subject)
        => $"Ticket number #{ticketNumber} - {subject}";

    public static string GetNewTicketPattern()
        => @"Ticket number #(\d+) - ";

    public static string GetNewTicketContent(string content)
        => $"A ticket has been created to which you have been assigned with the following content\n{content}";

    public static string GetNewUserSubject()
        => "Thank you for registration";

    public static string GetNewUserContent(string firstName, string lastName, string verificationToken)
        => $"Hello {firstName} {lastName}.\nThank you for registration. Here is your verification token:{verificationToken}";

    public static string GetAssigningSubject(int ticketNumber)
        => $"Ticket with number: {ticketNumber} has been assigned to you";
    
    public static string GetAssigningContent(int ticketNumber)
        => $"Ticket with number: {ticketNumber} has been assigned to you";
}