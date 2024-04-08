using System.Net;
using System.Text.RegularExpressions;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using wg.modules.messages.core.Clients.Companies;
using wg.modules.messages.core.Clients.Companies.DTO;
using wg.modules.messages.core.Entities;
using wg.modules.messages.core.Events.Mappers;
using wg.modules.messages.core.Services.Abstractions;
using wg.shared.abstractions.Mailbox;
using wg.shared.abstractions.Messaging;

namespace wg.modules.messages.core.Services;

internal sealed class MessageSearcher(
    ILogger<MessageSearcher> logger,
    IMailboxRegister mailboxRegister,
    ICompaniesApiClient companiesApiClient,
    IMessageBroker messageBroker) : IMessageSearcher
{
    public async Task SearchEmails(CancellationToken cancellationToken)
    {
        using var client = new ImapClient();
        await ConnectAsync(client, cancellationToken);
        
        var inbox = client.Inbox;
        var readFolder = await inbox.GetSubfolderAsync("Read", cancellationToken);
        var uids = await inbox.SearchAsync(SearchQuery.All, cancellationToken);
        var clientMessages = await GetClientMessages(uids, inbox, readFolder, cancellationToken);
        
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

    private async Task<List<ClientMessage>> GetClientMessages(IList<UniqueId> uids, IMailFolder inbox, IMailFolder subfolder, CancellationToken cancellationToken)
    {
        var clientMessages = new List<ClientMessage>();
        foreach (var uid in uids)
        {
            var message = await inbox.GetMessageAsync(uid, cancellationToken);
            var senderAddress = message.From.Mailboxes.Single().Address;
            if (IsAddressCorrect(senderAddress))
            {
                var employee = await companiesApiClient.GetEmployeeByEmailAsync(new EmployeeEmailDto(senderAddress));
                if (employee is not null && employee.IsActive)
                {
                    clientMessages.Add(ClientMessage.Create(message.Subject, message.TextBody, 
                        senderAddress, message.Date.DateTime, employee.Id));
                }
            }
            await inbox.MoveToAsync(uid, subfolder, cancellationToken);
        }

        return clientMessages;
    }

    private bool IsAddressCorrect(string address)
    {
        Regex regex = new(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.Compiled);
        return regex.IsMatch(address);
    }
}