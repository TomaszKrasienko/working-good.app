using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.domain.Repositories;

public interface IDailyEmployeeActivityRepository
{
    Task<DailyEmployeeActivity> GetByDateForUser(DateTime dateTime, Guid userId);
    Task AddAsync(DailyEmployeeActivity dailyEmployeeActivity);
    Task UpdateAsync(DailyEmployeeActivity dailyEmployeeActivity);
}