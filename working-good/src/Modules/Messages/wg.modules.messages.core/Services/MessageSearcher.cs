using System.Net;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using MimeKit;
using wg.modules.messages.core.Entities;
using wg.modules.messages.core.Events;
using wg.modules.messages.core.Events.Mappers;
using wg.modules.messages.core.Services.Abstractions;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Mailbox;
using wg.shared.abstractions.Messaging;

namespace wg.modules.messages.core.Services;

internal sealed class MessageSearcher(
    ILogger<MessageSearcher> logger,
    IMailboxRegister mailboxRegister,
    IMessageBroker messageBroker) : IMessageSearcher
{
    public async Task SearchEmails(CancellationToken cancellationToken)
    {
        using var client = new ImapClient();
        await ConnectAsync(client, cancellationToken);
        
        var inbox = client.Inbox;
        var readFolder = await inbox.GetSubfolderAsync("Read", cancellationToken);
        var uids = await inbox.SearchAsync(SearchQuery.All, cancellationToken);
        var clientMessages = new List<ClientMessage>();
        foreach (var uid in uids)
        {
            var message = await inbox.GetMessageAsync(uid, cancellationToken);
            clientMessages.Add(ClientMessage.Create(message.Subject, message.TextBody, 
                message.From.ToString(), message.Date.DateTime));
            await inbox.MoveToAsync(uid, readFolder, cancellationToken);
        }
        await client.DisconnectAsync(true, cancellationToken);

        var events = clientMessages?.Select(x => x.AsEvent()).ToArray();
        await messageBroker.PublishAsync(events);
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