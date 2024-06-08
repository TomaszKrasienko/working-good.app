using System.Collections.Immutable;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using wg.shared.infrastructure.Configuration;
using wg.shared.infrastructure.Vault.Configuration.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace wg.shared.infrastructure.Vault.Configuration;

public static class Extensions
{
    private const string SectionName = "Vault";

    public static IHostBuilder AddVault(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            
            var options = configuration.GetOptions<VaultOptions>(SectionName);
            var store = new VaultStore(options);
            var secret = store.GetAsync(options.Key).GetAwaiter().GetResult();
        
        
            var parser = new JsonParser();
            var json = JsonSerializer.Serialize(secret);
            var data = parser.Parse(json);
            var source = new MemoryConfigurationSource() { InitialData = data };
            cfg.Add(source);
        });
        return hostBuilder;
    }
}