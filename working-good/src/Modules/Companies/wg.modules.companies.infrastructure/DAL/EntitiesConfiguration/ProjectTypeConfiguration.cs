using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.ValueObjects.Project;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.companies.infrastructure.DAL.EntitiesConfiguration;

internal sealed class ProjectTypeConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasConversion(x => x.Value, y => new Title(y))
            .IsRequired()
            .HasMaxLength(500);

        builder
            .Property(x => x.Description)
            .HasConversion(x => x.Value, y => new Description(y));

        builder
            .Property(x => x.PlannedStart)
            .HasConversion(x => x.Value, y => new DurationTime(y));

        builder
            .Property(x => x.PlannedFinish)
            .HasConversion(x => x.Value, y => new DurationTime(y));
    }
}