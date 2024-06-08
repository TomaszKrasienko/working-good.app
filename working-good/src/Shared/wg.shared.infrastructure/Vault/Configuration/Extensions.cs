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
using wg.shared.infrastructure.Vault.Exceptions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace wg.shared.infrastructure.Vault.Configuration;

internal static class Extensions
{
    private const string SectionName = "Vault";

    internal static IHostBuilder AddVault(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var options = configuration.GetOptions<VaultOptions>(SectionName);
            if (!options.Enabled)
            {
                return;
            }
            var data = GetData(options);
            var source = new MemoryConfigurationSource() { InitialData = data };
            cfg.Add(source);
        });
        return hostBuilder;
    }

    private static IDictionary<string, string> GetData(VaultOptions options)
    {
        var store = new VaultStore(options);
        var secret = store.GetAsync(options.Key).GetAwaiter().GetResult();

        if (secret.Count == 0)
        {
            throw new ConfigurationException("Empty configuration");
        }
        
        var parser = JsonParser.Create();
        var json = JsonSerializer.Serialize(secret);
        return parser.Parse(json);
    }
}