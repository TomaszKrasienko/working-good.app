using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.ValueObjects;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.tickets.infrastructure.DAL.EntitiesConfiguration;

internal sealed class TicketTypeConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new AggregateId(y))
            .IsRequired();

        builder
            .Property(x => x.Number)
            .HasConversion(x => x.Value, y => new Number(y))
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

        builder
            .Property(x => x.CreatedBy)
            .HasConversion(x => x.Value, y => new CreatedBy(y))
            .IsRequired();

        builder
            .Property(x => x.IsPriority)
            .HasConversion(x => x.Value, y => new IsPriority(y))
            .IsRequired();

        builder
            .OwnsOne(x => x.Status, options =>
            {
                options
                    .Property(x => x.Value)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("State");
                options
                    .Property(x => x.ChangeDate)
                    .IsRequired()
                    .HasColumnName("StateChangeDate");
            });

        builder
            .Property(x => x.ExpirationDate)
            .HasConversion(x => x.Value, y => new ExpirationDate(y));
        
        builder
            .Property(x => x.AssignedEmployee)
            .HasConversion(x => x.Value, y => new EntityId(y));
        
        builder
            .Property(x => x.AssignedUser)
            .HasConversion(x => x.Value, y => new EntityId(y));
        
        builder
            .Property(x => x.ProjectId)
            .HasConversion(x => x.Value, y => new EntityId(y));

        builder
            .HasMany<Message>(x => x.Messages)
            .WithOne();
        
        builder
            .HasIndex(x => x.Number)
            .IsUnique();
    }
}