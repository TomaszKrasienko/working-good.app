namespace wg.shared.abstractions.Context;

public interface IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public string Role { get; }
}