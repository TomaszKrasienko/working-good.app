using Bogus;
using wg.modules.companies.domain.Entities;

namespace wg.sharedForTests.Factories.Companies;

public static class CompanyFactory
{
    public static Company Get()
    {
        var companyFaker = new Faker<Company>()
            .CustomInstantiator(f => Company.Create(
                Guid.NewGuid(),
                f.Company.CompanyName(),
                TimeSpan.FromHours(12),
                "test.pl"));
        return companyFaker.Generate(1).Single();
    }
}