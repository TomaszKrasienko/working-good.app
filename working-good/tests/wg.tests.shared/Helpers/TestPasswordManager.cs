using wg.modules.owner.application.Auth;

namespace wg.tests.shared.Helpers;

internal sealed class TestPasswordManager : IPasswordManager
{
    public string Secure(string password)
        => password;

    public bool VerifyPassword(string securedPassword, string password)
        => password == securedPassword;
}