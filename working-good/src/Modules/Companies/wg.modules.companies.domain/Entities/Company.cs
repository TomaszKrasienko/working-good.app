using System.Collections;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.companies.domain.Entities;

public sealed class Company : AggregateRoot
{
    public string Name { get; private set; }
    public TimeSpan SlaTime { get; private set; }
    public string EmailDomain { get; private set; }
    private readonly HashSet<Employee> _employees = new HashSet<Employee>();
    public IEnumerable<Employee> Employees => _employees;
    private readonly HashSet<Project> _projects = new HashSet<Project>();
    public IEnumerable<Project> Projects => _projects;
}