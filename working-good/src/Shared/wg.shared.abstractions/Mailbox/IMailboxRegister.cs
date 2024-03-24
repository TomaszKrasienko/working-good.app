namespace wg.shared.abstractions.Mailbox;

public interface IMailboxRegister
{
    public MailboxCredentials GetForReceiving();
    public MailboxCredentials GetForSending();
}