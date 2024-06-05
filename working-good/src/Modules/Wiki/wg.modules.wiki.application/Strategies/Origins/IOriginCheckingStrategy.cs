namespace wg.modules.wiki.application.Strategies.Origins;

internal interface IOriginCheckingStrategy
{
    bool CanByApply(string originType);
    Task<bool> IsExists(string originId);
}