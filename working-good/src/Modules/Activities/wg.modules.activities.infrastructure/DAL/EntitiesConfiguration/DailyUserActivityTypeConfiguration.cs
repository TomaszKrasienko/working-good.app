using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.ValueObjects.DailyUserActivity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.infrastructure.DAL.EntitiesConfiguration;

internal sealed class DailyUserActivityTypeConfiguration : IEntityTypeConfiguration<DailyUserActivity>
{
    public void Configure(EntityTypeBuilder<DailyUserActivity> builder)
    {
        builder.HasKey(x => new { x.Day, x.UserId});

        builder
            .Property(x => x.Day)
            .HasConversion(x => x.Value, y => new Day(y))
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasConversion(x => x.Value, y => new EntityId(y));

        builder
            .HasMany<Activity>(x => x.Activities)
            .WithOne();
    }
}