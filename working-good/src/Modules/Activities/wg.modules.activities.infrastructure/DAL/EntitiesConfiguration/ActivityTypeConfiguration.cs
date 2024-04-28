using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.ValueObjects.Activity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.infrastructure.DAL.EntitiesConfiguration;

internal sealed class ActivityTypeConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();

        builder
            .Property(x => x.Content)
            .HasConversion(x => x.Value, y => new Content(y))
            .IsRequired();

        builder
            .Property(x => x.TicketId)
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
            .HasDiscriminator<string>("type")
            .HasValue<PaidActivity>(nameof(PaidActivity))
            .HasValue<InternalActivity>(nameof(InternalActivity));
    }
}