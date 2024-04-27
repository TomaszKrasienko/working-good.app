using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.infrastructure.DAL.EntitiesConfiguration;

internal sealed class InternalActivityTypeConfiguration : IEntityTypeConfiguration<InternalActivity>
{
    public void Configure(EntityTypeBuilder<InternalActivity> builder)
    {
        
    }
}