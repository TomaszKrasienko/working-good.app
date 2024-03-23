using wg.modules.tickets.infrastructure.DAL;
using wg.tests.shared.Integration;

namespace wg.tests.shared.Db;

internal sealed class TestAppDb : IDisposable
{
    public TicketsDbContext TicketsDbContext { get; set; }

    public TestAppDb()
    {
        TicketsDbContext = new TicketsDbContext(DbContextOptionsProvider.GetDbContextOptions<TicketsDbContext>());
    }
    
    public void Dispose()
    {
        TicketsDbContext.Database.EnsureDeleted();
    }
}