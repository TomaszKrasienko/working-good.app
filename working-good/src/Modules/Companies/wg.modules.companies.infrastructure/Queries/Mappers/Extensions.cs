using wg.modules.companies.application.DTOs;
using wg.modules.companies.domain.Entities;

namespace wg.modules.companies.infrastructure.Queries.Mappers;

internal static class Extensions
{
    internal static CompanyDto AsDto(this Company company)
        => new CompanyDto()
        {
            Id = company.Id,
            Name = company.Name,
            SlaTime = company.SlaTime,
            EmailDomain = company.EmailDomain,
            Employees = company.Employees?.Select(x => x?.AsDto()).ToList(),
            Projects = company.Projects?.Select(x => x?.AsDto()).ToList()
        };

    internal static EmployeeDto AsDto(this Employee employee)
        => new EmployeeDto()
        {
            Id = employee.Id,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            IsActive = employee.IsActive
        };

    internal static ProjectDto AsDto(this Project project)
        => new ProjectDto()
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            PlannedStart = project.PlannedStart,
            PlannedFinish = project.PlannedFinish
        };
}