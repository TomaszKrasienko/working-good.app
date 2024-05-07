using Bogus;
using wg.modules.companies.domain.Entities;

namespace wg.tests.shared.Factories.Companies;

internal static class EmployeeFactory
{
    internal static Employee GetInCompany(Company company)
        => GetInCompany(company, 1).Single();
    
    internal static IEnumerable<Employee> GetInCompany(Company company, int count)
    {
        var employees = GetFaker(company.EmailDomain.Value).Generate(count);
        foreach (var employee in employees)
        {
            company.AddEmployee(employee.Id, employee.Email, employee.PhoneNumber);
        }
        
        return company.Employees;
    }

    internal static List<Employee> Get(int count = 1, string emailDomain = "test.pl")
        => GetFaker(emailDomain).Generate(count);
    
    private static Faker<Employee> GetFaker(string emailDomain)
        => new Faker<Employee>()
            .CustomInstantiator(f => Employee.Create(Guid.NewGuid(),
                $"{f.Name.FirstName()}.{f.Name.LastName()}@{emailDomain}", f.Phone.PhoneNumber()));
}