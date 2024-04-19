namespace wg.shared.infrastructure.Vault.Configuration.Models;

internal sealed record VaultOptions
{
    public string Url { get; set; }
    public string Secret { get; set; }
    public string Key { get; set; }
}