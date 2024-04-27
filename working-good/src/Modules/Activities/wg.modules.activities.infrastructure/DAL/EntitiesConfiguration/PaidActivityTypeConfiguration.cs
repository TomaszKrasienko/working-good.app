using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.infrastructure.DAL.EntitiesConfiguration;

internal sealed class PaidActivityTypeConfiguration : IEntityTypeConfiguration<PaidActivity>
{
    public void Configure(EntityTypeBuilder<PaidActivity> builder)
    {
        
    }
}