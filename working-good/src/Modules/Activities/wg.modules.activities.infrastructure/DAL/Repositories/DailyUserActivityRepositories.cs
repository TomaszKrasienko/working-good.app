using Microsoft.EntityFrameworkCore;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Repositories;

namespace wg.modules.activities.infrastructure.DAL.Repositories;

internal sealed class DailyUserActivityRepository(
    ActivitiesDbContext dbContext) : IDailyUserActivityRepository
{
    public async Task<DailyUserActivity> GetByDateForUser(DateTime dateTime, Guid userId)
    {
        var usersActivity = await dbContext
            .DailyUserActivities
            .Include(x => x.Activities)
            .Where(x=> x.UserId.Equals(userId))
            .ToListAsync();

        return usersActivity.FirstOrDefault(x => x.Day == dateTime);
    }


    public async Task AddAsync(DailyUserActivity dailyEmployeeActivity)
    {
        await dbContext.DailyUserActivities.AddAsync(dailyEmployeeActivity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(DailyUserActivity dailyEmployeeActivity)
    {
        dbContext.DailyUserActivities.Update(dailyEmployeeActivity);
        await dbContext.SaveChangesAsync();
    }
}