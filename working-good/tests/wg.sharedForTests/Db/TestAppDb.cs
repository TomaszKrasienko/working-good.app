using wg.modules.tickets.infrastructure.DAL;
using wg.sharedForTests.Integration;

namespace wg.sharedForTests.Db;

internal sealed class TestAppDb : IDisposable
{
    public TicketsDbContext TicketsDbContext { get; set; }

    public TestAppDb()
    {
        TicketsDbContext = new TicketsDbContext(DbContextOptionsProvider.GetDbContextOptions<TicketsDbContext>());
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
}