using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.companies.domain.Entities;

public sealed class Employee
{
    public EntityId Id { get; }
    public Email Email { get; set; }
    public string PhoneNumber { get; set; }
}