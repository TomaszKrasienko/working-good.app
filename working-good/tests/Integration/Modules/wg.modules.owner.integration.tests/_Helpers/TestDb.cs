using Microsoft.EntityFrameworkCore;
using wg.modules.owner.infrastructure.DAL;
using wg.tests.shared.Integration;

namespace wg.modules.owner.integration.tests._Helpers;

internal sealed class TestDb : IDisposable
{
    public OwnerDbContext OwnerDbContext { get; }

    public TestDb()
    {
        OwnerDbContext = new OwnerDbContext(DbContextOptionsProvider.GetDbContextOptions<OwnerDbContext>());
    }

    public void Dispose()
    {
        OwnerDbContext.Database.EnsureDeleted();
        OwnerDbContext.Dispose();
    }
}