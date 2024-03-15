using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace wg.shared.infrastructure.DAL;

internal sealed class DbInitializer(
    IServiceProvider serviceProvider, 
    ILogger<DbInitializer> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting initializing database");

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var dbContexts = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsAssignableTo(typeof(DbContext)) && typeof(DbContext) != x);
            
        using var scope = serviceProvider.CreateScope();
        foreach (var dbContext in dbContexts)
        {
            var context = (DbContext)scope.ServiceProvider.GetService(dbContext);
            if (context is null)
            {
                continue;
            }
            await context?.Database.MigrateAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}