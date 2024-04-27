using Bogus;
using wg.modules.activities.domain.Entities;

namespace wg.tests.shared.Factories.Activities;

internal static class ActivityFactory
{
    internal static PaidActivity GetPaidActivity(DateTime timeFrom, DateTime? timeTo)
        => GetPaidActivityFaker(timeFrom, timeTo).Generate(1).Single();
    
    internal static InternalActivity GetInternalActivity(DateTime timeFrom, DateTime? timeTo)
        => GetInternalActivityFaker(timeFrom, timeTo).Generate(1).Single();
    
    internal static Activity GetRandom(DateTime timeFrom, DateTime? timeTo)
    {
        var isPaid = new Random().Next(10) % 2 == 0;
        if (isPaid)
        {
            var paidFaker = GetPaidActivityFaker(timeFrom, timeTo);
            return paidFaker.Generate(1).Single();
        }
        else
        {
            var paidFaker = GetInternalActivityFaker(timeFrom, timeTo);
            return paidFaker.Generate(1).Single();
        }
    }
    
    private static Faker<PaidActivity> GetPaidActivityFaker(DateTime timeFrom, DateTime? timeTo)
        => new Faker<PaidActivity>()
            .CustomInstantiator(v
                => PaidActivity.Create(Guid.NewGuid(), v.Lorem.Sentence(null, 
                4), Guid.NewGuid(), timeFrom, timeTo));
    
    private static Faker<InternalActivity> GetInternalActivityFaker(DateTime timeFrom, DateTime? timeTo)
        => new Faker<InternalActivity>()
            .CustomInstantiator(v
                => InternalActivity.Create(Guid.NewGuid(), v.Lorem.Sentence(null, 
                    4), Guid.NewGuid(), timeFrom, timeTo));
}