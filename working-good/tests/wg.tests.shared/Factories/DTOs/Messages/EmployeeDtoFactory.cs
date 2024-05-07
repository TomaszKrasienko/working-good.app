using Bogus;
using wg.modules.messages.core.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Messages;

internal static class EmployeeDtoFactory
{
    internal static EmployeeDto Get(bool isActive = true)
        => Get(1, isActive).Single();
    
    private static List<EmployeeDto> Get(int count, bool isActive = true)
    {
        var faker = GetFaker(isActive);
        return faker.Generate(count);
    }
    
    private static Faker<EmployeeDto> GetFaker(bool isActive)
        => new Faker<EmployeeDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Email, v => v.Internet.Email())
            .RuleFor(f => f.PhoneNumber, v => v.Phone.PhoneNumber())
            .RuleFor(f => f.IsActive, v => isActive);
}