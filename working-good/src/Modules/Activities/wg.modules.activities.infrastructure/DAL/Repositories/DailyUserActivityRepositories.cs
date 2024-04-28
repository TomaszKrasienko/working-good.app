using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Repositories;

namespace wg.modules.activities.infrastructure.DAL.Repositories;

internal sealed class DailyUserActivityRepository : IDailyUserActivityRepository
{
    public Task<DailyUserActivity> GetByDateForUser(DateTime dateTime, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(DailyUserActivity dailyEmployeeActivity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(DailyUserActivity dailyEmployeeActivity)
    {
        throw new NotImplementedException();
    }
}