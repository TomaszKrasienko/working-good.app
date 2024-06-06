using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.ValueObjects.Note;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.wiki.infrastructure.DAL.EntitiesConfiguration;

internal sealed class NoteTypeConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasConversion(x => x.Value, y => new Title(y))
            .IsRequired();

        builder
            .Property(x => x.Content)
            .HasConversion(x => x.Value, y => new Content(y))
            .IsRequired();

        builder.OwnsOne(x => x.Origin, options =>
        {
            options
                .Property(y => y.Id)
                .HasColumnName("OriginId");

            options
                .Property(y => y.Type)
                .HasColumnName("OriginType");
        });
    }
}