using Microsoft.EntityFrameworkCore;
using wg.modules.wiki.core.Entities;

namespace wg.modules.wiki.core.DAL;

internal sealed class WikiDbContext : DbContext
{
    public DbSet<Section> Sections { get; set; }

    public WikiDbContext(DbContextOptions<WikiDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("wiki");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}