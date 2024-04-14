using Bogus;
using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Company;

internal static class EmployeeDtoFactory
{
    internal static List<EmployeeDto> Get(string emailDomain = null, int count = 1)
        => GetFaker(emailDomain).Generate(count);
    
    internal static Faker<EmployeeDto> GetFaker(string emailDomain = null)
        => new Faker<EmployeeDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Email, v => string.IsNullOrWhiteSpace(emailDomain) 
                ? v.Internet.Email() 
                : $"{v.Name.FirstName().ToLowerInvariant()}.{v.Name.LastName().ToLowerInvariant()}@{emailDomain}")
            .RuleFor(f => f.IsActive, true)
            .RuleFor(f => f.PhoneNumber, v => v.Phone.PhoneNumber());
}