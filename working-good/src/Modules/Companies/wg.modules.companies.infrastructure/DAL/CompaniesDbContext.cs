using Microsoft.EntityFrameworkCore;
using wg.modules.companies.domain.Entities;

namespace wg.modules.companies.infrastructure.DAL;

public sealed class CompaniesDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }

    public CompaniesDbContext(DbContextOptions<CompaniesDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("companies");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}