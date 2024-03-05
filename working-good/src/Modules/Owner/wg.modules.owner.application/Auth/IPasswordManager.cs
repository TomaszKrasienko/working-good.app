namespace wg.modules.owner.application.Auth;

public interface IPasswordManager
{
    string Secure(string password);
    bool VerifyPassword(string securedPassword, string password);
}