using Microsoft.EntityFrameworkCore;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Repositories;

namespace wg.modules.activities.infrastructure.DAL.Repositories;

internal sealed class DailyUserActivityRepository(
    ActivitiesDbContext dbContext) : IDailyUserActivityRepository
{
    public Task<DailyUserActivity> GetByDateForUser(DateTime dateTime, Guid userId)
        => dbContext
            .DailyUserActivities
            .Include(x => x.Activities)
            .FirstOrDefaultAsync(x => x.Day.Value == dateTime.Date && x.UserId.Equals(userId));


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