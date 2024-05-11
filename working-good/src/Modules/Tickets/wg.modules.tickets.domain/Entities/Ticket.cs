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
    //TODO: Add change priority with unit tests and checking employee and user
    public Number Number { get; }
    public Subject Subject { get; private set; }
    public Content Content { get; private set; }
    public CreatedAt CreatedAt { get; }
    public CreatedBy CreatedBy { get; }
    public IsPriority IsPriority { get; private set; }
    public Status Status { get; private set; }
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

    public static Ticket Create(Guid id, int number, string subject, string content, DateTime createdAt,
        string createdBy)
    {
        var ticket = new Ticket(id, number, createdAt, createdBy);
        ticket.ChangeSubject(subject);
        ticket.ChangeContent(content);
        ticket.ChangeStatus(Status.New(), createdAt);
        return ticket;
    }

    private void ChangeSubject(string subject)
        => Subject = subject;

    private void ChangeContent(string content)
        => Content = content;

    public void ChangeStatus(string state, DateTime changeDate)
    {
        //TODO: Add tests for changes not while creating
        var statePolicy = TicketStatePolicy.Create();
        if (Status is null || statePolicy.CanChangeState(Status))
        {
            Status = new Status(state, changeDate);
        }
    }

    internal void ChangeAssignedEmployee(Guid assignedEmployee)
    {
        //TODO: Add unit tests
        var statePolicy = TicketStatePolicy.Create();
        if (statePolicy.CanChangeState(Status))
        {
            AssignedEmployee = assignedEmployee;
        }
    }

    public void ChangeAssignedUser(Guid assignedUser, DateTime stateChangeDate)
    {
        //TODO: Add unit tests
        var statePolicy = TicketStatePolicy.Create();
        if (!statePolicy.CanChangeState(Status)) return;
        AssignedUser = assignedUser;
    }

    public void RemoveAssignedUser()
    {
        //TODO: Add unit tests
        var statePolicy = TicketStatePolicy.Create();
        if (!statePolicy.CanChangeState(Status)) return;
        AssignedUser = null;
    }

    //TODO: Add unit tests
    public void ChangeProject(Guid projectId)
        => ProjectId = projectId;

    public void AddMessage(Guid id, string sender, string subject, string content,
        DateTime createdAt)
    {
        Status = new Status(Status.WaitingForResponse(), createdAt);
        _messages.Add(Message.Create(id, sender, subject, content, createdAt));
    }
}