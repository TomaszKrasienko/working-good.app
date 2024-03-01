namespace wg.shared.abstractions.Kernel.Types;

public record EntityId<T>(T Value)
{
    
}

public sealed record EntityId : EntityId<Guid>
{
    public EntityId(Guid value) : base(value)
    {
        
    }

    public EntityId() : this(Guid.NewGuid())
    {
        
    }

    public bool IsEmpty() => Value == Guid.NewGuid();
} 