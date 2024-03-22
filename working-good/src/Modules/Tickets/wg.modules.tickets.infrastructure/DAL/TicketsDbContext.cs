using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.infrastructure.DAL;

internal sealed class TicketsDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Message> Messages { get; set; }

    public TicketsDbContext(DbContextOptions<TicketsDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tickets");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}