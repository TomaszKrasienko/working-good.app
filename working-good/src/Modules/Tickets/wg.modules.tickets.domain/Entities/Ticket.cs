using System;
using System.Collections.Generic;
using System.Linq;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Policies;
using wg.modules.tickets.domain.ValueObjects;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.domain.Entities;

public sealed class Ticket : AggregateRoot<AggregateId>
{
    public Number Number { get; private set; }
    public Subject Subject { get; private set; }
    public Content Content { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public CreatedBy CreatedBy { get; private set; }
    public IsPriority IsPriority { get; private set; }
    public State State { get; private set; }
    public ExpirationDate ExpirationDate { get; private set; }
    public EntityId AssignedEmployee { get; private set; }
    public EntityId AssignedUser { get; private set; }
    public EntityId ProjectId  { get; private set; }
    private List<Message> _messages = new List<Message>();
    public IReadOnlyList<Message> Messages => _messages;
    
    private Ticket(AggregateId id, Number number, CreatedAt createdAt, CreatedBy createdBy)
    {
        Id = id;
        Number = number;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }
    
    private Ticket(AggregateId id, Number number, Subject subject, Content content, CreatedAt createdAt, 
        CreatedBy createdBy, IsPriority isPriority, State state, ExpirationDate expirationDate, 
        EntityId assignedEmployee, EntityId assignedUser, EntityId projectId) : this(id, number, createdAt, createdBy)
    {
        Subject = subject;
        Content = content;
        IsPriority = isPriority;
        ExpirationDate = expirationDate;
        AssignedEmployee = assignedEmployee;
        AssignedUser = assignedUser;
        State = state;
        ProjectId = projectId;
    }

    public static Ticket Create(Guid id, int number, string subject, string content, DateTime createdAt,
        string createdBy, string state, DateTime stateChangeDate, bool isPriority, DateTime? expirationDate = null, Guid? assignedEmployee = null,
        Guid? assignedUser = null, Guid? projectId = null, string employeeEmail = null)
    {
        var ticket = new Ticket(id, number, createdAt, createdBy);
        ticket.ChangeSubject(subject);
        ticket.ChangeContent(content);
        ticket.ChangeState(state, stateChangeDate);
        ticket.ChangePriority(isPriority, expirationDate);
        if (assignedEmployee is not null)
        {
            ticket.ChangeAssignedEmployee((Guid)assignedEmployee);
        }

        if (assignedUser is not null)
        {
            ticket.ChangeAssignedUser((Guid)assignedUser, stateChangeDate);
        }

        if (projectId is not null)
        {
            ticket.ChangeProject((Guid)projectId);
        }
        return ticket;
    }

    private void ChangeSubject(string subject)
        => Subject = subject;

    private void ChangeContent(string content)
        => Content = content;

    public void ChangeState(string state, DateTime changeDate)
    {
        var statePolicy = TicketStatePolicy.Create();
        if (State is null || statePolicy.CanChangeState(State))
        {
            State = new State(state, changeDate);
        }
    }

    private void ChangePriority(bool priority, DateTime? expirationDate)
    {
        if (priority && expirationDate is null)
        {
            throw new MissingExpirationDateException(true);
        }

        IsPriority = priority;
        ExpirationDate = expirationDate;
    }

    internal void ChangeAssignedEmployee(Guid assignedEmployee)
    {        
        var statePolicy = TicketStatePolicy.Create();
        if (statePolicy.CanChangeState(State))
        {
            AssignedEmployee = assignedEmployee;
        }
    }

    public void ChangeAssignedUser(Guid assignedUser, DateTime stateChangeDate)
    {
        var statePolicy = TicketStatePolicy.Create();
        if (!statePolicy.CanChangeState(State)) return;
        AssignedUser = assignedUser;
        if (State == State.New())
        {
            ChangeState(State.Open(), stateChangeDate);
        }
    }

    public void ChangeProject(Guid projectId)
        => ProjectId = projectId;

    public void AddMessage(Guid id, string sender, string subject, string content,
        DateTime createdAt)
    {
        State = new State(State.Open(), createdAt);
        _messages.Add(Message.Create(id, sender, subject, content, createdAt));
    }
}