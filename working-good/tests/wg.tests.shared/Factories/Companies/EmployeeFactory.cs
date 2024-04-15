using Bogus;
using wg.modules.companies.domain.Entities;

namespace wg.tests.shared.Factories.Companies;

internal static class EmployeeFactory
{
    internal static IEnumerable<Employee> GetEmployeeInCompany(int count = 1, Company company = null)
    {
        var faker = GetFaker(company?.EmailDomain.Value);
        var employees = faker.Generate(count);
        foreach (var employee in employees)
        {
            company?.AddEmployee(employee.Id, employee.Email, employee.PhoneNumber);
        }
        return company?.Employees;
    }

    internal static List<Employee> Get(int count = 1, string emailDomain = "test.pl")
        => GetFaker(emailDomain).Generate(count);
    
    private static Faker<Employee> GetFaker(string emailDomain)
        => new Faker<Employee>()
            .CustomInstantiator(f => Employee.Create(Guid.NewGuid(),
                $"{f.Name.FirstName()}.{f.Name.LastName()}@{emailDomain}", f.Phone.PhoneNumber()));
}