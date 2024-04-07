using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.ValueObjects.Company;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.companies.infrastructure.DAL.EntitiesConfiguration;

internal sealed class CompanyTypeConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new AggregateId(y))
            .IsRequired();
        
        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, y => new Name(y))
            .IsRequired();
        
        builder
            .Property(x => x.SlaTime)
            .HasConversion(x => x.Value, y => new SlaTime(y))
            .IsRequired();
        
        builder
            .Property(x => x.EmailDomain)
            .HasConversion(x => x.Value, y => new EmailDomain(y))
            .IsRequired()
            .HasMaxLength(20);

        builder
            .Property(x => x.IsActive)
            .HasConversion(x => x.Value, y => new IsActive(y))
            .IsRequired();
        
        builder
            .HasMany<Employee>(x => x.Employees)
            .WithOne();
        
        builder
            .HasMany<Project>(x => x.Projects)
            .WithOne();
    }
}