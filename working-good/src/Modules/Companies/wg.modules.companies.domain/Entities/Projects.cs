using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.domain.ValueObjects.Project;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.companies.domain.Entities;

public sealed class Project
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public DurationTime PlannedStart { get; private set; }
    public DurationTime PlannedFinish { get; private set; }

    private Project(EntityId id, Title title, Description description, DurationTime plannedStart, 
        DurationTime plannedFinish)
    {
        Id = id;
        Title = title;
        Description = description;
        PlannedStart = plannedStart;
        PlannedFinish = plannedFinish;
    }
    
    private Project(EntityId id)
    {
        Id = id;
    }

    internal static Project Create(Guid id, string title, string description, DateTime? plannedStart = null,
        DateTime? plannedFinish = null)
    {
        var project = new Project(id);
        project.ChangeTitle(title);
        project.ChangeDescription(description);
        project.ChangePlannedStart(plannedStart);
        project.ChangePlannedFinish(plannedFinish);
        return project;
    }

    internal void ChangeTitle(string title)
        => Title = title;

    internal void ChangeDescription(string description)
        => Description = description;

    internal void ChangePlannedStart(DateTime? plannedStart)
    {
        if (!IsDurationValid(plannedStart, PlannedFinish))
        {
            throw new InvalidDurationTimeException((DateTime)plannedStart!, (DateTime)PlannedFinish);
        }

        PlannedStart = plannedStart;
    }
    
    internal void ChangePlannedFinish(DateTime? plannedFinish)
    {
        if (!IsDurationValid(PlannedStart, plannedFinish))
        {
            throw new InvalidDurationTimeException((DateTime)PlannedStart, (DateTime)plannedFinish!);
        }

        PlannedFinish = plannedFinish;
    }

    private bool IsDurationValid(DateTime? plannedStart, DateTime? plannedFinish)
    {
        if (plannedStart is null || plannedFinish is null)
        {
            return true;
        }

        if (plannedStart < plannedFinish)
        {
            return true;
        }

        return false;
    } 
}