using Microsoft.EntityFrameworkCore;
using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.infrastructure.DAL;

internal sealed class ActivitiesDbContext
{
    public DbSet<DailyEmployeeActivity> DailyEmployeeActivities { get; set; }
}