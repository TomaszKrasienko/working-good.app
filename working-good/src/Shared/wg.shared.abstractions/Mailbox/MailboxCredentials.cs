namespace wg.shared.abstractions.Mailbox;

public sealed record MailboxCredentials
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string Server { get; init; }
    public int Port { get; init; }
}