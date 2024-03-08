using Microsoft.EntityFrameworkCore;
using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.infrastructure.DAL;

internal sealed class OwnerDbContext : DbContext
{
    public DbSet<Owner> Owner { get; set; }
    public DbSet<User> Users { get; set; }

    public OwnerDbContext(DbContextOptions<OwnerDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("owner");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}