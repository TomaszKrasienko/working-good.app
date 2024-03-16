using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.owner.domain.Entities;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.infrastructure.DAL.EntitiesConfiguration;

internal sealed class GroupTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();
        builder
            .Property(x => x.Title)
            .HasConversion(x => x.Value, y => new Title(y))
            .IsRequired();
        builder
            .HasMany<User>(x => x.Users)
            .WithMany(x => x.Groups)
            .UsingEntity("GroupMembership");
    }
}