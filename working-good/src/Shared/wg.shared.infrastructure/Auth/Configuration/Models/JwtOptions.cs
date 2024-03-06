namespace wg.shared.infrastructure.Auth.Configuration.Models;

internal sealed record JwtOptions
{    
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public TimeSpan Expiry { get; init; }
    public string SigningKey { get; init; }
}