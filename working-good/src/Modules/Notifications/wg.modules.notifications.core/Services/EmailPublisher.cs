using System.Net;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Mailbox;

namespace wg.modules.notifications.core.Services;

internal sealed class EmailPublisher(
    ILogger<EmailPublisher> logger,
    IMailboxRegister mailboxRegister) : IEmailPublisher
{
    public async Task PublishAsync(EmailNotification emailNotification, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();
        await ConnectAsync(client, cancellationToken);
        MimeMessage mimeMessage = GetMessage(emailNotification.Recipient,
            mailboxRegister.GetForSending().Username, emailNotification.Subject,
            emailNotification.Content);
        await client.SendAsync(mimeMessage, cancellationToken);
    }

    private async Task ConnectAsync(SmtpClient client, CancellationToken cancellationToken)
    {
        var mailboxCredentials = mailboxRegister.GetForSending();
        var credentials = new NetworkCredential(mailboxCredentials.Username, mailboxCredentials.Password);
        await client.ConnectAsync(mailboxCredentials.Server, mailboxCredentials.Port, cancellationToken: cancellationToken);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(credentials, cancellationToken);
    }
    
    private MimeMessage GetMessage(List<string> recipient, string sender, string subject, string content)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(sender));
        recipient.ForEach(r => message.To.Add(MailboxAddress.Parse(r)));
        message.Subject = subject;
        var builder = new BodyBuilder
        {
            TextBody = content
        };
        message.Body = builder.ToMessageBody();
        return message;
    }
}