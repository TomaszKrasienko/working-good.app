using wg.modules.activities.infrastructure.DAL;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.tickets.infrastructure.DAL;
using wg.tests.shared.Integration;

namespace wg.tests.shared.Db;

internal sealed class TestAppDb : IDisposable
{
    public TicketsDbContext TicketsDbContext { get; }
    public CompaniesDbContext CompaniesDbContext { get; }
    public OwnerDbContext OwnerDbContext { get; }
    public ActivitiesDbContext ActivitiesDbContext { get; set; }

    public TestAppDb()
    {
        TicketsDbContext = new TicketsDbContext(DbContextOptionsProvider.GetDbContextOptions<TicketsDbContext>());
        CompaniesDbContext = new CompaniesDbContext(DbContextOptionsProvider.GetDbContextOptions<CompaniesDbContext>());
        OwnerDbContext = new OwnerDbContext(DbContextOptionsProvider.GetDbContextOptions<OwnerDbContext>());
        ActivitiesDbContext = new ActivitiesDbContext(DbContextOptionsProvider.GetDbContextOptions<ActivitiesDbContext>());
    }
    
    public void Dispose()
    {
        TicketsDbContext.Database.EnsureDeleted();
        CompaniesDbContext.Database.EnsureDeleted();
        OwnerDbContext.Database.EnsureDeleted();
        ActivitiesDbContext.Database.EnsureDeleted();
        TicketsDbContext.Dispose();
        CompaniesDbContext.Dispose();
        OwnerDbContext.Dispose();
        ActivitiesDbContext.Dispose();
    }
}