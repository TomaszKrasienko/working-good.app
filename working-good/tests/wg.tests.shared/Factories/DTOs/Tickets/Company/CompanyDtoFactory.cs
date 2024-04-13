using Bogus;
using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Company;

internal static class CompanyDtoFactory
{
    internal static List<CompanyDto> Get(int count = 1, string emailDomain = null, 
        List<EmployeeDto> employeeDtoList = null, List<ProjectDto> projectDtoList = null)
    {
        var companyDtoFaker = new Faker<CompanyDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Name, v => v.Lorem.Word())
            .RuleFor(f => f.SlaTime, v => v.Date.Timespan(TimeSpan.FromDays(2)))
            .RuleFor(f => f.EmailDomain, v 
                => string.IsNullOrWhiteSpace(emailDomain) 
                    ? v.Internet.DomainName()
                    : emailDomain);

        var companies = companyDtoFaker.Generate(count);

        if (employeeDtoList is not null)
        {
            foreach (var company in companies)
            {
                company.Employees.AddRange(employeeDtoList);
            }
        }

        if (projectDtoList is not null)
        {
            foreach (var company in companies)
            {
                company.Projects.AddRange(projectDtoList);
            }
        }

        return companies;
    }
}