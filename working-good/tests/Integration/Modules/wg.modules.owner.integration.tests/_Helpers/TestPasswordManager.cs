using wg.modules.owner.application.Auth;

namespace wg.modules.owner.integration.tests._Helpers;

internal sealed class TestPasswordManager : IPasswordManager
{
    public string Secure(string password)
        => password;

    public bool VerifyPassword(string securedPassword, string password)
        => password == securedPassword;
}