namespace wg.modules.companies.application.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TimeSpan SlaTime { get; set; }
    public string EmailDomain { get; set; }
    public List<EmployeeDto> Employees { get; set; }
    public List<ProjectDto> Projects { get; set; }
}