using System.Net;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
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
        await ConnectAsync(client, cancellationToken);
        
        var inbox = client.Inbox;
        var readFolder = await inbox.GetSubfolderAsync("Read", cancellationToken);
        var uids = await inbox.SearchAsync(SearchQuery.All, cancellationToken);
        foreach (var uid in uids)
        {
            var message = await inbox.GetMessageAsync(uid, cancellationToken);
            await inbox.MoveToAsync(uid, readFolder, cancellationToken);
        }
        await client.DisconnectAsync(true, cancellationToken);
    }

    private async Task ConnectAsync(ImapClient client, CancellationToken cancellationToken)
    {
        var mailboxCredentials = mailboxRegister.GetForReceiving();
        var credentials = new NetworkCredential(mailboxCredentials.Username, mailboxCredentials.Password);
        await client.ConnectAsync(mailboxCredentials.Server, mailboxCredentials.Port, cancellationToken: cancellationToken);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(credentials, cancellationToken);
        await client.Inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);
    }
}