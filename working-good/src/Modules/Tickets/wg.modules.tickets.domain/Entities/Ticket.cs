using wg.modules.tickets.domain.Exceptions;
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
    public State State { get; private set; }
    public ExpirationDate ExpirationDate { get; private set; }
    public EntityId AssignedEmployee { get; private set; }
    public EntityId AssignedUser { get; private set; }

    private Ticket(AggregateId id, Number number, CreatedAt createdAt, EntityId createdBy)
    {
        Id = id;
        Number = number;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }
    
    private Ticket(AggregateId id, Number number, Subject subject, Content content, CreatedAt createdAt, 
        EntityId createdBy, IsPriority isPriority, State state, ExpirationDate expirationDate, 
        EntityId assignedEmployee, EntityId assignedUser) : this(id, number, createdAt, createdBy)
    {
        Subject = subject;
        Content = content;
        IsPriority = isPriority;
        ExpirationDate = expirationDate;
        AssignedEmployee = assignedEmployee;
        AssignedUser = assignedUser;
        State = state;
    }

    internal static Ticket Create(Guid id, int number, string subject, string content, DateTime createdAt,
        Guid? createdBy, string state, DateTime stateChange, bool isPriority, DateTime? expirationDate = null, Guid? assignedEmployee = null,
        Guid? assignedUser = null)
    {
        var ticket = new Ticket(id, number, createdAt, createdBy);
        ticket.ChangeSubject(subject);
        ticket.ChangeContent(content);
        ticket.ChangeState(state, stateChange);
        ticket.ChangePriority(isPriority, expirationDate);
        if (assignedEmployee is not null)
        {
            ticket.ChangeAssignedEmployee((Guid)assignedEmployee);
        }

        if (assignedUser is not null)
        {
            ticket.ChangeAssignedUser((Guid)assignedUser);
        }

        return ticket;
    }

    private void ChangeSubject(string subject)
        => Subject = subject;

    private void ChangeContent(string content)
        => Content = content;

    private void ChangeState(string state, DateTime changeDate)
        => State = new State(state, changeDate);

    private void ChangePriority(bool priority, DateTime? expirationDate)
    {
        if (priority && expirationDate is null)
        {
            throw new MissingExpirationDateException(true);
        }

        IsPriority = priority;
        ExpirationDate = expirationDate;
    }

    private void ChangeAssignedEmployee(Guid assignedEmployee)
        => AssignedEmployee = assignedEmployee;

    private void ChangeAssignedUser(Guid assignedUser)
        => AssignedUser = assignedUser;
}