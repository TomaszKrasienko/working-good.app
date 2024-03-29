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
    public Guid AssignedEmployee { get; set; }
    public int? Number { get; private set; }

    private ClientMessage(Guid id, string subject, string content, string sender, DateTime createdAt,
        Guid assignedEmployee)
    {
        Id = id;
        Subject = subject;
        Content = content;
        Sender = sender;
        CreatedAt = createdAt;
        AssignedEmployee = assignedEmployee;
    }

    private ClientMessage(Guid id, string subject, string content, string sender, DateTime createdAt,
        Guid assignedEmployee, int number)
        : this(id, subject, content, sender, createdAt, assignedEmployee)
    {
        Number = number;
    }

    internal static ClientMessage Create(string subject, string content, string sender, DateTime createdAt,
        Guid assignedEmployee)
    {
        var clientMessage = new ClientMessage(Guid.NewGuid(), subject, content, sender, createdAt, assignedEmployee);
        clientMessage.SetNumber();
        return clientMessage;
    }

    private void SetNumber()
    {
        string pattern = @"Ticket number #(\d+) - ";
        if (Regex.IsMatch(Subject, pattern))
        {
            var sentenceMatch = Regex.Match(Subject, pattern);
            var numberPattern = @"(\d+)";
            var numberMatch = Regex.Match(sentenceMatch.Value, numberPattern);
            Number = int.Parse(numberMatch.Value);
        }
    }
}