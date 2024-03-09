using Microsoft.EntityFrameworkCore;
using wg.shared.infrastructure.DAL.Configuration.Models;

namespace wg.sharedForTests.Integration;

public static class DbContextOptionsProvider
{
    public static DbContextOptions<T> GetDbContextOptions<T>() where T : DbContext
    {
        var optionsProvider = new OptionsProvider();
        var options = optionsProvider.Get<DalOptions>("DAL");
        return new DbContextOptionsBuilder<T>()
            .UseSqlServer(options.ConnectionString)
            .EnableSensitiveDataLogging()
            .Options;
    }
}