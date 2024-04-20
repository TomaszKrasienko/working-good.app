using Microsoft.Extensions.Options;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;
using wg.shared.infrastructure.Vault.Configuration.Models;

namespace wg.shared.infrastructure.Vault;

internal sealed class VaultStore(VaultOptions options)
{
    private readonly VaultOptions _options = options;
    public async Task<IDictionary<string, object>> GetAsync(string key)
    {
        var settings = new VaultClientSettings(options.Url, new TokenAuthMethodInfo(_options.Secret));
        // var settings =
        //     new VaultClientSettings(options.Url, new UserPassAuthMethodInfo("kv", "wg", "StrongPassword123"));
        var client = new VaultClient(settings);
        var secret = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(key, mountPoint:"kv");
        return secret.Data.Data;
    }
}