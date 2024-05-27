using Bogus;
using wg.modules.notifications.core.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Notifications;

internal static class EmployeeDtoFactory
{
    internal static EmployeeDto Get()
        => Get(1).Single();
    
    internal static List<EmployeeDto> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<EmployeeDto> GetFaker()
        => new Faker<EmployeeDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Email, v => v.Internet.Email())
            .RuleFor(f => f.PhoneNumber, v => v.Phone.PhoneNumber());
}