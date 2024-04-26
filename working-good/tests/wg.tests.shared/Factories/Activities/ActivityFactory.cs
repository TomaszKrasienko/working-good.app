using Bogus;
using wg.modules.activities.domain.Entities;

namespace wg.tests.shared.Factories.Activities;

internal static class ActivityFactory
{
    internal static Activity GetRandom(DateTime timeFrom, DateTime? timeTo)
    {
        var isPaid = new Random().Next(10) % 2 == 0;
        if (isPaid)
        {
            var paidFaker = new Faker<PaidActivity>()
                .CustomInstantiator(v
                    => PaidActivity.Create(Guid.NewGuid(), v.Lorem.Sentence(null, 
                            4), Guid.NewGuid(), timeFrom, timeTo));
            return paidFaker.Generate(1).Single();
        }
        else
        {
            var paidFaker = new Faker<InternalActivity>()
                .CustomInstantiator(v
                    => InternalActivity.Create(Guid.NewGuid(), v.Lorem.Sentence(null, 
                        4), Guid.NewGuid(), timeFrom, timeTo));
            return paidFaker.Generate(1).Single();
        }
    }
}