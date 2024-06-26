using Bogus;
using wg.modules.activities.domain.Entities;

namespace wg.tests.shared.Factories.Activities;

internal static class DailyUserActivityFactory
{
    internal static DailyUserActivity Get()
        => Get(1).Single();
    
    private static List<DailyUserActivity> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<DailyUserActivity> GetFaker()
        => new Faker<DailyUserActivity>().CustomInstantiator(v
            => DailyUserActivity.Create(DateTime.Now, Guid.NewGuid()));
}