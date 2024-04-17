using Bogus;
using wg.modules.tickets.domain.Entities;

namespace wg.tests.shared.Factories.Tickets;

internal static class ActivityFactory
{
    internal static IEnumerable<Activity> GetInTicket(Ticket ticket, int count = 1, bool isPaid = true, Guid? userId = null)
    {
        var faker = GetFaker(isPaid, userId);
        var activities = faker.Generate(count);
        foreach (var activity in activities)
        {
            ticket.AddActivity(activity.Id, activity.ActivityTime.TimeFrom, activity.ActivityTime.TimeTo,
                activity.Note, activity.IsPaid, activity.UserId);
        }

        return ticket.Activities;
    }

    internal static List<Activity> Get(int count = 1, bool? isPaid = null)
    {
        isPaid = isPaid ?? (new Random().Next() % 2 == 0 ? true : false);
        return GetFaker((bool)isPaid!, Guid.NewGuid()).Generate(count);
    }
    
    private static Faker<Activity> GetFaker(bool isPaid = true, Guid? userId = null)
        => new Faker<Activity>().CustomInstantiator(v => Activity.Create(Guid.NewGuid(), v.Date.Recent(),
            v.Date.Soon(), v.Lorem.Sentence(), isPaid, userId ?? Guid.NewGuid()));
}