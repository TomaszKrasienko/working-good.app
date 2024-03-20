using Bogus;
using wg.modules.companies.domain.Entities;

namespace wg.sharedForTests.Factories.Companies;

public static class EmployeeFactory
{
    public static Employee GetEmployeeInCompany(Company company)
    {
        var employeeFaker = new Faker<Employee>()
            .CustomInstantiator(f => Employee.Create
            (
                Guid.NewGuid(),
                $"{f.Name.FirstName()}.{f.Name.LastName()}@{company.EmailDomain.Value}",
                f.Phone.PhoneNumber())
            );
        var employee = employeeFaker.Generate(1).Single();
        company.AddEmployee(employee.Id, employee.Email, employee.PhoneNumber);
        return company.Employees.Single(x => x.Id == employee.Id);
    }
}