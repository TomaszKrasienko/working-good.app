using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.ValueObjects.Activity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.infrastructure.DAL.EntitiesConfiguration;

public sealed class ActivityTypeConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();

        builder
            .OwnsOne(x => x.ActivityTime, options =>
            {
                options
                    .Property(x => x.TimeFrom)
                    .IsRequired()
                    .HasColumnName("ActivityTimeFrom");
                options
                    .Property(x => x.TimeTo)
                    .HasColumnName("ActivityTimeTo");
            });

        builder
            .Property(x => x.Note)
            .HasConversion(x => x.Value, y => new Note(y))
            .IsRequired();

        builder
            .Property(x => x.IsPaid)
            .HasConversion(x => x.Value, y => new IsPaid(y))
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();
    }
}