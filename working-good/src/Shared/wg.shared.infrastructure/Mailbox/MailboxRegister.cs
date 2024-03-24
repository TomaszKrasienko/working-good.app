using Microsoft.Extensions.Options;
using wg.shared.abstractions.Mailbox;
using wg.shared.infrastructure.Mailbox.Configuration.Models;

namespace wg.shared.infrastructure.Mailbox;

internal sealed class MailboxRegister : IMailboxRegister
{
    private readonly MailboxOptions _options;

    public MailboxRegister(IOptions<MailboxOptions> options)
        => _options = options.Value;


    public MailboxCredentials GetForReceiving()
        => new MailboxCredentials()
        {
            Username = _options.Username,
            Password = _options.Password,
            Server = _options.ReceivingServer,
            Port = _options.ReceivingPort,
        };

    public MailboxCredentials GetForSending()
    {
        throw new NotImplementedException();
    }
}