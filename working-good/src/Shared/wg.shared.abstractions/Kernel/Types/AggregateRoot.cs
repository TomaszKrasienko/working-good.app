namespace wg.shared.abstractions.Kernel.Types;

public abstract class AggregateRoot<T>
{
    public T Id { get; protected set; }
}

public abstract class AggregateRoot : AggregateRoot<AggregateId>
{
    
}