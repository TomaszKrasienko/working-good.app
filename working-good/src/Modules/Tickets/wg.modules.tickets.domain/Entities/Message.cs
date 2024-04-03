using System;
using wg.modules.tickets.domain.ValueObjects;
using wg.modules.tickets.domain.ValueObjects.Message;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.domain.Entities;

public sealed class Message 
{
    public EntityId Id { get; }
    public Sender Sender { get; }
    public Subject Subject { get; }
    public Content Content { get;  }
    public CreatedAt CreatedAt { get; } 
    private Message(EntityId id, Sender sender, Subject subject, Content content, 
        CreatedAt createdAt)
    {
        Id = id;
        Sender = sender;
        Subject = subject;
        Content = content;
        CreatedAt = createdAt;
    }

    internal static Message Create(Guid id, string sender, string subject, string content,
        DateTime createdAd)
        => new Message(id, sender, subject, content, createdAd);
}