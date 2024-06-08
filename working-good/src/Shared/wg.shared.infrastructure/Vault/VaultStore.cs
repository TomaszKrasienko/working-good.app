using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;
using wg.shared.infrastructure.Vault.Configuration.Models;
using wg.shared.infrastructure.Vault.Exceptions;

namespace wg.shared.infrastructure.Vault;

internal sealed class VaultStore(VaultOptions options)
{
    public async Task<IDictionary<string, object>> GetAsync(string key)
    {

        var settings = new VaultClientSettings(options.Url, GetAuthMethodInfo());
        //var settings = new VaultClientSettings(options.Url, new TokenAuthMethodInfo(options.Secret));
        var client = new VaultClient(settings);
        var secret = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(key, mountPoint:"kv");
        return secret.Data.Data;
    }

    private AbstractAuthMethodInfo GetAuthMethodInfo()
    {
        if (!string.IsNullOrWhiteSpace(options.Auth?.Secret))
        {
            return new TokenAuthMethodInfo(options.Auth.Secret);
        }

        if (!string.IsNullOrWhiteSpace(options.Auth?.Password)
            && !string.IsNullOrWhiteSpace(options.Auth?.Username))
        {
            return new UserPassAuthMethodInfo(options.Auth?.Username, options.Auth?.Password);
        }

        throw new ConfigurationException("Auth info is empty");
    }
}