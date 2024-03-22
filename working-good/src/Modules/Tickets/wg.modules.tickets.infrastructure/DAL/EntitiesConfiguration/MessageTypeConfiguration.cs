using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.ValueObjects;
using wg.modules.tickets.domain.ValueObjects.Sender;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.infrastructure.DAL.EntitiesConfiguration;

internal sealed class MessageTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {        
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();

        builder
            .Property(x => x.Sender)
            .HasConversion(x => x.Value, y => new Sender(y))
            .IsRequired();
        
        builder
            .Property(x => x.Subject)
            .HasConversion(x => x.Value, y => new Subject(y))
            .HasMaxLength(300)
            .IsRequired();
        
        builder
            .Property(x => x.Content)
            .HasConversion(x => x.Value, y => new Content(y))
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .HasConversion(x => x.Value, y => new CreatedAt(y))
            .IsRequired();
    }
}