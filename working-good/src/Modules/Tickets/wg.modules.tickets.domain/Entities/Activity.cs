using System;
using wg.modules.tickets.domain.ValueObjects.Activity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.domain.Entities;

public sealed class Activity
{
    public EntityId Id { get; }
    public ActivityTime ActivityTime { get; private set; }
    public Note Note { get; private set; }
    public IsPaid IsPaid { get; private set; }
    public EntityId UserId { get; private set; }

    private Activity(EntityId id)
    {
        Id = id;
    }

    internal static Activity Create(Guid id, DateTime timeFrom, DateTime? timeTo, 
        string note, bool isPaid, EntityId userId)
    {
        var activity = new Activity(id);
        if (isPaid)
        {
            activity.MarkAsPaid();
        }
        else
        {
            activity.MarkAsNoPaid();
        }
        activity.ChangeNote(note);
        if (timeTo is null)
        {
            activity.ChangeActivityTime(timeFrom);
        }
        else
        {
            activity.ChangeActivityTime(timeFrom, (DateTime)timeTo);
        }
        activity.ChangeUser(userId);
        return activity;
    }

    private void ChangeActivityTime(DateTime timeFrom, DateTime timeTo)
        => ActivityTime = new ActivityTime(timeFrom, timeTo);
    
    private void ChangeActivityTime(DateTime timeFrom)
        => ActivityTime = new ActivityTime(timeFrom);

    private void ChangeNote(string note)
        => Note = note;

    private void ChangeUser(Guid userId)
        => UserId = userId;

    internal void ChangeType()
    {
        if (IsPaid)
            MarkAsNoPaid();
        else
            MarkAsPaid();
    }

    private void MarkAsPaid()
        => IsPaid = true;

    private void MarkAsNoPaid()
        => IsPaid = false;
}