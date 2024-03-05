using Microsoft.AspNetCore.Identity;
using wg.modules.owner.application.Auth;
using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.infrastructure.Auth;

internal sealed class PasswordManager
    (IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(default!, password);

    public bool VerifyPassword(string securedPassword, string password)
        => passwordHasher
               .VerifyHashedPassword(default!, securedPassword, password) == PasswordVerificationResult.Success;
}