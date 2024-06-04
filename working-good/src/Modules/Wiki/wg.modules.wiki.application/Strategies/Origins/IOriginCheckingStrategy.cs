namespace wg.modules.wiki.application.Strategies.Origins;

internal interface IOriginCheckingStrategy
{
    Task<bool> IsExists(string originId);
}