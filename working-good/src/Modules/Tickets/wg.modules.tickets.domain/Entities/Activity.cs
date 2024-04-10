using System;
using Microsoft.AspNetCore.Http.HttpResults;
using wg.modules.tickets.domain.ValueObjects.Activity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.domain.Entities;

public sealed class Activity
{
    public EntityId Id { get; }
    public ActivityTime ActivityTime { get; private set; }
    public Note Note { get; private set; }
    public IsPaid IsPaid { get; private set; }

    private Activity(EntityId id)
    {
        Id = id;
    }

    internal static Activity Create(Guid id, DateTime timeFrom, DateTime timeTo, 
        string note, bool isPaid)
    {
        var activity = Create(id, note, isPaid);
        activity.ChangeActivityTime(timeFrom, timeTo);
        return activity;
    }

    internal static Activity Create(Guid id, DateTime timeFrom, string note, bool isPaid)
    {
        var activity = Create(id, note, isPaid);
        activity.ChangeActivityTime(timeFrom);
        return activity;
    }
    
    private static Activity Create(Guid id,string note, bool isPaid)
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
        return activity;
    }

    private void ChangeActivityTime(DateTime timeFrom, DateTime timeTo)
        => ActivityTime = new ActivityTime(timeFrom, timeTo);
    
    private void ChangeActivityTime(DateTime timeFrom)
        => ActivityTime = new ActivityTime(timeFrom);

    private void ChangeNote(string note)
        => Note = note;

    private void MarkAsPaid()
        => IsPaid = true;

    private void MarkAsNoPaid()
        => IsPaid = false;
}