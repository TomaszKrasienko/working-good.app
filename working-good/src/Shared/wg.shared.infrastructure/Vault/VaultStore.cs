using Microsoft.Extensions.Options;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using wg.shared.infrastructure.Vault.Configuration.Models;

namespace wg.shared.infrastructure.Vault;

internal sealed class VaultStore(VaultOptions options)
{
    private readonly VaultOptions _options;
    
    public async Task<IDictionary<string, object>> GetAsync(string key)
    {
        var settings = new VaultClientSettings(_options.Url, new TokenAuthMethodInfo(_options.Secret));
        var client = new VaultClient(settings);
        var secret = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(key);
        return secret.Data.Data;
    }
}