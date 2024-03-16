using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.owner.domain.Entities;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.infrastructure.DAL.EntitiesConfiguration;

internal sealed class OwnerTypeConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new AggregateId(y))
            .IsRequired();
        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, y => new Name(y))
            .IsRequired()
            .HasMaxLength(40);
        builder
            .HasMany<User>(x => x.Users)
            .WithOne();
        builder
            .HasMany<Group>(x => x.Groups)
            .WithOne();
    }
}