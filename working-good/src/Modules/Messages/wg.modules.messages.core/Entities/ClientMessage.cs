using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.messages.core.Entities;

internal sealed class ClientMessage
{
    public EntityId Id { get; }
    public string Subject { get; }
    public string Content { get; }
    public string Sender { get;  }
    public DateTime CreatedAt { get; }
    public int? Number { get; private set; }

    private ClientMessage(Guid id, string subject, string content, string sender, DateTime createdAt)
    {
        Id = id;
        Subject = subject;
        Content = content;
        Sender = sender;
        CreatedAt = createdAt;
    }

    private ClientMessage(Guid id, string subject, string content, string sender, DateTime createdAt, int number)
        : this(id, subject, content, sender, createdAt)
    {
        Number = number;
    }

    internal static ClientMessage Create(string subject, string content, string sender, DateTime createdAt)
    {
        var clientMessage = new ClientMessage(Guid.NewGuid(), subject, content, sender, createdAt);
        clientMessage.SetNumber();
        return clientMessage;
    }

    private void SetNumber()
    {
        string pattern = @"^#[0-9]+";
        if (Regex.IsMatch(Subject, pattern))
        {
            var number = Regex.Match(Subject, pattern);
            Number = int.Parse(number.Value.Substring(1));
        }
    }
}