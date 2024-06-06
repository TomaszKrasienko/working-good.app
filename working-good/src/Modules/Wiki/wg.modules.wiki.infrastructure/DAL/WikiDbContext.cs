using Microsoft.EntityFrameworkCore;
using wg.modules.wiki.domain.Entities;

namespace wg.modules.wiki.infrastructure.DAL;

internal sealed class WikiDbContext : DbContext
{
    public DbSet<Section> Sections { get; set; }
    public DbSet<Note> Notes { get; set; }

    public WikiDbContext(DbContextOptions<WikiDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("wiki");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}