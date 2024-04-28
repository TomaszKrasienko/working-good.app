using Microsoft.EntityFrameworkCore;
using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.infrastructure.DAL;

internal sealed class ActivitiesDbContext : DbContext
{
    public DbSet<DailyUserActivity> DailyUserActivities { get; set; }
    public DbSet<Activity> Activities { get; set; }

    public ActivitiesDbContext(DbContextOptions<ActivitiesDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("activities");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}