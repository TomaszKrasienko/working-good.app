using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.companies.domain.Entities;

public sealed class Company : AggregateRoot
{
    public string Name { get; private set; }
    public TimeSpan SlaTime { get; private set; }
    public string EmailDomain { get; private set; }
}