using wg.modules.companies.infrastructure.DAL;
using wg.tests.shared.Integration;

namespace wg.modules.companies.integration.tests._Helpers;

internal sealed class TestDb : IDisposable
{
    public CompaniesDbContext CompaniesDbContext { get; }

    public TestDb()
    {
        CompaniesDbContext = new CompaniesDbContext(DbContextOptionsProvider.GetDbContextOptions<CompaniesDbContext>());
    }

    public void Dispose()
    {
        CompaniesDbContext.Database.EnsureDeleted();
        CompaniesDbContext.Dispose();
    }
}