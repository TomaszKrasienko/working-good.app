using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.infrastructure.Configuration;
using wg.shared.infrastructure.Vault.Configuration.Models;

namespace wg.shared.infrastructure.Vault.Configuration;

internal static class Extensions
{
    private const string SectionName = "Vault";
    
    internal static IServiceCollection AddVault(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<VaultOptions>(SectionName);
        var store = new VaultStore(options);
        var secret = store.GetAsync(options.Key).GetAwaiter().GetResult();
        var parser = new JsonParser()
    }
}