using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.domain.Repositories;

public interface IDailyUserActivityRepository
{
    Task<DailyUserActivity> GetByDateForUser(DateTime dateTime, Guid userId);
    Task AddAsync(DailyUserActivity dailyEmployeeActivity);
    Task UpdateAsync(DailyUserActivity dailyEmployeeActivity);
}