using System.Security.Cryptography;

namespace wg.modules.owner.domain.ValueObjects.User;

public sealed record VerificationToken
{
    public string Token { get; }
    public DateTimeOffset? VerificationDate { get; set; }

    private VerificationToken()
    {
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "").Replace("==", "");
    }

    internal static VerificationToken Create()
        => new VerificationToken();

    internal void Verify(DateTime dateTime)
        => VerificationDate = dateTime;
}