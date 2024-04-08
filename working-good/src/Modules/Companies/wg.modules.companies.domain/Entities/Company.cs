using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.domain.ValueObjects.Company;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.companies.domain.Entities;

public sealed class Company : AggregateRoot
{
    public Name Name { get; private set; }
    public SlaTime SlaTime { get; private set; }
    public EmailDomain EmailDomain { get; private set; }
    public IsActive IsActive { get; private set; }
    private readonly HashSet<Employee> _employees = new HashSet<Employee>();
    public IEnumerable<Employee> Employees => _employees;
    private readonly HashSet<Project> _projects = new HashSet<Project>();
    public IEnumerable<Project> Projects => _projects;

    private Company(AggregateId id)
    {
        Id = id;
    }

    public static Company Create(Guid id, string name, TimeSpan slaTime, string emailDomain)
    {
        var company = new Company(id);
        company.ChangeName(name);
        company.ChangeSlaTime(slaTime);
        company.ChangeEmailDomain(emailDomain);
        company.Activate();
        return company;
    }

    private void ChangeName(string name)
        => Name = name;

    private void ChangeSlaTime(TimeSpan slaTime)
        => SlaTime = slaTime;

    private void ChangeEmailDomain(string emailDomain)
        => EmailDomain = emailDomain;

    private void Activate()
        => IsActive = true;

    internal void Deactivate()
        => IsActive = false;

    public void AddEmployee(Guid id, string email, string phoneNumber = null)
    {
        if (!IsActive)
        {
            throw new CompanyNotActiveException(Id);
        }
        
        if (_employees.Any(x => x.Email == email))
        {
            throw new EmailAlreadyInUseException(email);
        }

        if (!email.EndsWith(EmailDomain))
        {
            throw new EmailNotMatchToEmailDomainException(email, EmailDomain);
        }

        var employee = Employee.Create(id, email, phoneNumber);
        _employees.Add(employee);
    }

    public void DeactivateEmployee(Guid id)
    {
        var employee = _employees.FirstOrDefault(x => x.Id.Equals(id));
        if (employee is null)
        {
            throw new EmployeeNotFoundException(id);
        }
        
        employee.Deactivate();
    }

    public void AddProject(Guid id, string title, string description, DateTime? plannedStart, DateTime? plannedFinish)
    {
        if (!IsActive)
        {
            throw new CompanyNotActiveException(Id);
        }
        
        if (_projects.Any(x => x.Title.Value.ToLowerInvariant() == title.ToLowerInvariant()))
        {
            throw new ProjectAlreadyRegisteredException(title);
        }

        _projects.Add(Project.Create(id, title, description, plannedStart, plannedFinish));
    }

    public void EditProject(Guid id, string title, string description, DateTime? plannedStart, DateTime? plannedFinish)
    {
        var project = _projects.FirstOrDefault(x => x.Id.Equals(id));
        if (project is null)
        {
            throw new ProjectNotFoundException(id);
        }
        
        project.ChangeTitle(title);
        project.ChangeDescription(description);
        if (plannedStart is not null)
        {
            project.ChangePlannedStart(plannedStart);
        }

        if (plannedFinish is not null)
        {
            project.ChangePlannedFinish(plannedFinish);
        }
    } 
}