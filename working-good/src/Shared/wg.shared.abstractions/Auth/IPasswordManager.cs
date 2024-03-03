namespace wg.shared.abstractions.Auth;

public interface IPasswordManager
{
    string Secure(string password);
}