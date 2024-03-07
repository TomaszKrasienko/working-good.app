using wg.modules.companies.domain.ValueObjects.Employee;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.companies.domain.Entities;

public sealed class Employee
{
    public EntityId Id { get; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private Employee(EntityId id)
    {
        Id = id;
    }

    internal static Employee Create(Guid id, string email, string phoneNumber = null)
    {
        var employee = new Employee(id);
        employee.ChangeEmail(email);
        employee.ChangePhoneNumber(phoneNumber);
        return employee;
    }
    
    private void ChangeEmail(string email)
        => Email = email;

    private void ChangePhoneNumber(string phoneNumber)
        => PhoneNumber = phoneNumber;
}