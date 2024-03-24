using System.Net;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Logging;
using wg.modules.messages.core.Services.Abstractions;
using wg.shared.abstractions.Mailbox;

namespace wg.modules.messages.core.Services;

internal sealed class MessageSearcher(
    ILogger<MessageSearcher> logger,
    IMailboxRegister mailboxRegister) : IMessageSearcher
{
    private static int _count = 0;
    
    public async Task SearchEmails(CancellationToken cancellationToken)
    {
        using var client = new ImapClient();
        var mailboxCredentials = mailboxRegister.GetForReceiving();
        var credentials = new NetworkCredential(mailboxCredentials.Username, mailboxCredentials.Password);
        await client.ConnectAsync(mailboxCredentials.Server, mailboxCredentials.Port, cancellationToken: cancellationToken);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(credentials, cancellationToken);
        await client.Inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken);
        var inbox = client.Inbox;
        _count = inbox.Count;
        
        client.Inbox.CountChanged += ((sender, e) =>
        {
            logger.LogInformation(inbox.Count.ToString());
        });

        await client.DisconnectAsync(true, cancellationToken);
    }
}