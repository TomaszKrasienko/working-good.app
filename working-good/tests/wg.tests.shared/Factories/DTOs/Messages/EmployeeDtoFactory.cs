using Bogus;
using wg.modules.messages.core.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Messages;

internal static class EmployeeDtoFactory
{
    internal static List<EmployeeDto> Get(int count = 1, bool isActive = true)
    {
        var faker = new Faker<EmployeeDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Email, v => v.Internet.Email())
            .RuleFor(f => f.PhoneNumber, v => v.Phone.PhoneNumber())
            .RuleFor(f => f.IsActive, v => isActive);

        return faker.Generate(count);
    }
}