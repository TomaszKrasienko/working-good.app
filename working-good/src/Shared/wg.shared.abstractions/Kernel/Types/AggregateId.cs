namespace wg.shared.abstractions.Kernel.Types;

public record AggregateId<T>(T Value)
{
}

public sealed record AggregateId : AggregateId<Guid>
{
    public AggregateId(Guid value) : base(Guid.NewGuid()) { }
    
    public AggregateId() : this(Guid.NewGuid()) { }

    public static implicit operator Guid(AggregateId id) => id.Value;
    public static implicit operator AggregateId(Guid value) => new AggregateId(value);
}