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
        return company;
    }

    private void ChangeName(string name)
        => Name = name;

    private void ChangeSlaTime(TimeSpan slaTime)
        => SlaTime = slaTime;

    private void ChangeEmailDomain(string emailDomain)
        => EmailDomain = emailDomain;

    public void AddEmployee(Guid id, string email, string phoneNumber = null)
    {
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
}