using Bogus;
using wg.modules.companies.domain.Entities;

namespace wg.tests.shared.Factories.Companies;

internal static class CompanyFactory
{
    internal static Company Get(string emailDomain = null)
        => Get(1, emailDomain).Single();
    
    internal static List<Company> Get(int count, string emailDomain = null)
        => GetFaker(string.IsNullOrWhiteSpace(emailDomain) ? "test.pl" : emailDomain).Generate(count);
    
     private static Faker<Company> GetFaker(string emailDomain)
        => new Faker<Company>()
            .CustomInstantiator(f => Company.Create(Guid.NewGuid(), f.Company.CompanyName(),
                TimeSpan.FromHours(12), emailDomain));
}