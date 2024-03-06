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
}