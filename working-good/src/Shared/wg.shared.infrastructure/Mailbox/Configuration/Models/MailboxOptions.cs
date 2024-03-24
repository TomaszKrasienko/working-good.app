namespace wg.shared.infrastructure.Mailbox.Configuration.Models;

internal sealed record MailboxOptions
{
    public string Username { get; init; }
    public string Password { get; init; }    
    public string ReceivingServer { get; init; }
    public int ReceivingPort { get; init; }
    public string SendingServer { get; init; }
    public int SendingPort { get; init; }
}