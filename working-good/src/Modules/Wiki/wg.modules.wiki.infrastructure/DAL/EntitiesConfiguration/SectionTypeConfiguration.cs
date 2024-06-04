using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.ValueObjects.Section;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.wiki.infrastructure.DAL.EntitiesConfiguration;

internal sealed class SectionTypeConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, y => new Name(y))
            .IsRequired();
        
        builder
            .HasOne<Section>(x => x.Parent)
            .WithMany();
        
        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}