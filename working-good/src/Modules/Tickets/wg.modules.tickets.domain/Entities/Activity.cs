using System;
using Microsoft.AspNetCore.Http.HttpResults;
using wg.modules.tickets.domain.ValueObjects.Activity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.domain.Entities;

public sealed class Activity
{
    public EntityId Id { get; }
    public ActivityTime ActivityTime { get; private set; }
    public Note Note { get; }
    public IsPaid IsPaid { get; }

    private Activity(EntityId id)
    {
        Id = id;
    }

    internal static Activity Create(Guid id, DateTime timeFrom, DateTime timeTo, 
        string note, bool isPaid)
    {
        var activity = new Activity(id);
        return activity;
    }
}