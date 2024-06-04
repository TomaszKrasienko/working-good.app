namespace wg.modules.wiki.application.Strategies.Origins;

internal sealed class ClientCheckingStrategy : IOriginCheckingStrategy
{
    public Task<bool> IsExists(string originId)
    {
        throw new NotImplementedException();
    }
}