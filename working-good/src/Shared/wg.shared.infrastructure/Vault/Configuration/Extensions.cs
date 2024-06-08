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

namespace wg.shared.infrastructure.Vault.Configuration;

public static class Extensions
{
    private const string SectionName = "Vault";

    public static IHostBuilder AddVault(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var options = configuration.GetOptions<VaultOptions>(SectionName);
        var store = new VaultStore(options);
        var secret = store.GetAsync(options.Key).GetAwaiter().GetResult();
        var parser = new JsonParser();
        var json = JsonConvert.SerializeObject(secret);
        hostBuilder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            cfg.AddJsonStream(stream);
        });
        return hostBuilder;

        // var data = parser.Parse(json);
        // var configurationBuilder = new ConfigurationBuilder();
        //
        // using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        //
        //     // Dodawanie konfiguracji JSON z strumienia
        //     configurationBuilder.AddJsonStream(stream);
        //
        //
        // var test = configurationBuilder.Build();
        //
        // var source = new MemoryConfigurationSource()
        // {
        //     InitialData = data
        // };
        // hostBuilder.Configuration.AddInMemoryCollection(data);
        // // hostBuilder.ConfigureServices(services =>
        // // {
        // //
        // // }).ConfigureAppConfiguration((cfg, ctx) =>
        // // {
        // //     ctx.Add(source);
        // // });
        // return hostBuilder;
    }
}