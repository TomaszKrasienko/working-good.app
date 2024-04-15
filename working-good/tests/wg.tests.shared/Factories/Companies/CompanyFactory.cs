using Bogus;
using wg.modules.companies.domain.Entities;

namespace wg.tests.shared.Factories.Companies;

public static class CompanyFactory
{
    public static List<Company> Get(int count = 1, string emailDomain = "test.pl")
        => GetFaker(emailDomain).Generate(count);
    
     private static Faker<Company> GetFaker(string emailDomain)
        => new Faker<Company>()
            .CustomInstantiator(f => Company.Create(Guid.NewGuid(), f.Company.CompanyName(),
                TimeSpan.FromHours(12), emailDomain));
}