using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.infrastructure.DAL.EntitiesConfiguration;

internal sealed class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();
        builder
            .Property(x => x.Email)
            .HasConversion(x => x.Value, y => new Email(y))
            .IsRequired()
            .HasMaxLength(40);
        builder
            .OwnsOne(x => x.FullName, options =>
            {
                options
                    .Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(40);
                options
                    .Property(x => x.LastName)
                    .IsRequired()
                    .HasMaxLength(60);
            });
        builder
            .Property(x => x.Password)
            .HasConversion(x => x.Value, y => new Password(y))
            .IsRequired();
        builder
            .Property(x => x.Role)
            .HasConversion(x => x.Value, y => new Role(y))
            .IsRequired();
        builder
            .OwnsOne(x => x.VerificationToken, options =>
            {
                options
                    .Property(x => x.Token)
                    .IsRequired()
                    .HasMaxLength(100);
                options
                    .Property(x => x.VerificationDate);
            });
        builder
            .OwnsOne(x => x.ResetPasswordToken, options =>
            {
                options
                    .Property(x => x.Token)
                    .HasMaxLength(100);
                options
                    .Property(x => x.Expiry);
            });
        builder
            .Property(x => x.State)
            .HasConversion(x => x.Value, y => new State(y))
            .IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}