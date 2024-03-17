using wg.modules.tickets.domain.ValueObjects;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.domain.Entities;

public sealed class Ticket : AggregateRoot
{
    public Number Number { get; private set; }
    public Subject Subject { get; private set; }
    public Content Content { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public EntityId CreatedBy { get; private set; }
    public IsPriority IsPriority { get; private set; }
    public ExpirationDate ExpirationDate { get; private set; }
    public EntityId AssignedEmployee { get; private set; }
    public EntityId AssignedUser { get; private set; }
    public State State { get; private set; }

    private Ticket(AggregateId id, CreatedAt createdAt, EntityId createdBy)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }
    
    private Ticket(AggregateId id, Subject subject, Content content, CreatedAt createdAt, 
        EntityId createdBy, IsPriority isPriority, ExpirationDate expirationDate, 
        EntityId assignedEmployee, EntityId assignedUser, State state) : this(id, createdAt, createdBy)
    {
        Subject = subject;
        Content = content;
        IsPriority = isPriority;
        ExpirationDate = expirationDate;
        AssignedEmployee = assignedEmployee;
        AssignedUser = assignedUser;
        State = state;
    }

}