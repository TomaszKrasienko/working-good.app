using Bogus;
using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Company;

internal static class CompanyDtoFactory
{
    internal static List<CompanyDto> Get(int count = 1, string emailDomain = null, 
        int? employeesCount = null, int? projectsCount = null)
    {
        var companies = GetFaker(emailDomain).Generate(count);

        if (employeesCount is not null)
        {
            foreach (var companyDto in companies)
            {
                var employees = EmployeeDtoFactory.Get(companyDto.EmailDomain, (int)employeesCount);
                companyDto.Employees.AddRange(employees);
            }
        }

        if (projectsCount is not null)
        {
            foreach (var company in companies)
            {
                var projects = ProjectDtoFactory.Get(false, false, (int)projectsCount);
                company.Projects.AddRange(projects);
            }
        }

        return companies;
    }
    
    internal static Faker<CompanyDto> GetFaker(string emailDomain = null)
        => new Faker<CompanyDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Name, v => v.Lorem.Word())
            .RuleFor(f => f.SlaTime, v => v.Date.Timespan(TimeSpan.FromDays(2)))
            .RuleFor(f => f.EmailDomain, v => string.IsNullOrWhiteSpace(emailDomain) 
                ? v.Internet.DomainName()
                : emailDomain);
}