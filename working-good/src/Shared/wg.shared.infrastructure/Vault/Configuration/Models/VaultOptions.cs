namespace wg.shared.infrastructure.Vault.Configuration.Models;

internal sealed record VaultOptions
{
    public bool Enabled { get; init; }
    public string Url { get; init; }
    public string Key { get; init; }
    public VaultAuthOptions Auth { get; set; }
}

internal sealed record VaultAuthOptions
{
    public string Secret { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}