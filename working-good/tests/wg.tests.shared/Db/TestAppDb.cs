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

    public TestAppDb()
    {
        TicketsDbContext = new TicketsDbContext(DbContextOptionsProvider.GetDbContextOptions<TicketsDbContext>());
        CompaniesDbContext = new CompaniesDbContext(DbContextOptionsProvider.GetDbContextOptions<CompaniesDbContext>());
        OwnerDbContext = new OwnerDbContext(DbContextOptionsProvider.GetDbContextOptions<OwnerDbContext>());
    }
    
    public void Dispose()
    {
        TicketsDbContext.Database.EnsureDeleted();
        CompaniesDbContext.Database.EnsureDeleted();
        OwnerDbContext.Database.EnsureDeleted();
        TicketsDbContext.Dispose();
        CompaniesDbContext.Dispose();
        OwnerDbContext.Dispose();
    }
}