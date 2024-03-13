using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.ValueObjects.Employee;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.companies.infrastructure.DAL.EntitiesConfiguration;

internal sealed class EmployeeTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
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
            .Property(x => x.PhoneNumber)
            .HasConversion(x => x.Value, y => new PhoneNumber(y))
            .HasMaxLength(40);

        builder
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}