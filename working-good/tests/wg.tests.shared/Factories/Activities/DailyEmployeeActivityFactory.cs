using Bogus;
using wg.modules.activities.domain.Entities;

namespace wg.tests.shared.Factories.Activities;

internal static class DailyEmployeeActivityFactory
{
    internal static DailyEmployeeActivity Get()
        => Get(1).Single();
    
    internal static List<DailyEmployeeActivity> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<DailyEmployeeActivity> GetFaker()
        => new Faker<DailyEmployeeActivity>().CustomInstantiator(v
            => DailyEmployeeActivity.Create(Guid.NewGuid(), DateTime.Now, Guid.NewGuid()));
}