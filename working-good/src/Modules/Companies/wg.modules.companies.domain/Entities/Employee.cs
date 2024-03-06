using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.companies.domain.Entities;

public sealed class Employee
{
    public EntityId Id { get; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}