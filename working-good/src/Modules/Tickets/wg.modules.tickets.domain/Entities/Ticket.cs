using System;
using System.Collections.Generic;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Policies;
using wg.modules.tickets.domain.ValueObjects;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.domain.Entities;

public sealed class Ticket : AggregateRoot<AggregateId>
{
    //TODO: Add change priority with unit tests
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
        ticket.ChangePriority(false);
        return ticket;
    }

    public void ChangeSubject(string subject)
        => Subject = subject;

    public void ChangeContent(string content)
        => Content = content;

    public void ChangeStatus(string state, DateTime changeDate)
    {
        var statePolicy = TicketStatePolicy.Create();
        if (Status is null || statePolicy.CanChangeState(Status))
        {
            Status = new Status(state, changeDate);
        }
    }

    public void ChangeAssignedEmployee(Guid assignedEmployee)
    {
        var statePolicy = TicketStatePolicy.Create();
        if (statePolicy.CanChangeState(Status))
        {
            AssignedEmployee = assignedEmployee;
        }
    }

    public void ChangeAssignedUser(Guid assignedUser)
    {
        var statePolicy = TicketStatePolicy.Create();
        if (statePolicy.CanChangeState(Status))
        {
            AssignedUser = assignedUser;
        }
    }

    public void RemoveAssignedUser()
    {
        var statePolicy = TicketStatePolicy.Create();
        if (statePolicy.CanChangeState(Status))
        {
            AssignedUser = null;
        }
    }

    public void ChangeProject(Guid projectId)
    {
        var statePolicy = TicketStatePolicy.Create();
        if (statePolicy.CanChangeState(Status))
        {
            ProjectId = projectId;
        }
    }

    public void ChangePriority(bool isPriority, TimeSpan? slaTime = null, DateTime? now = null)
    {
        if (isPriority)
        {
            if (AssignedEmployee is null)
            {
                throw new MissingAssignedEmployeeException(Id);
            }

            if (slaTime is null || slaTime <= TimeSpan.Zero)
            {
                throw new InvalidSlaTimeForTicketException(Id, slaTime);
            }

            if (now is null || now <= DateTime.MinValue)
            {
                throw new InvalidNowTimeForChangingPriorityException();
            }

            var newExpirationDate = now.Value.Add(slaTime.Value);
            if (ExpirationDate is null || ExpirationDate.Value > newExpirationDate)
            {
                ExpirationDate = new ExpirationDate(now.Value.Add(slaTime.Value));   
            }
        }

        IsPriority = new IsPriority(isPriority);
    }

    internal void ChangeExpirationDate(DateTime value)
    {
        ExpirationDate = value;
    }

    public void AddMessage(Guid id, string sender, string subject, string content,
        DateTime createdAt, bool isFromUser)
    {
        Status = isFromUser ? new Status(Status.WaitingForResponse(), createdAt) : new Status(Status.CustomerReplied(), createdAt);
        _messages.Add(Message.Create(id, sender, subject, content, createdAt));
    }
}