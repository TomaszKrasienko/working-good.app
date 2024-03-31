using Microsoft.Extensions.Logging;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.shared.infrastructure.Logging.Decorators;

[Decorator]
internal sealed class QueryHandlerLogDecorator<TQuery, TResponse>(
    IQueryHandler<TQuery, TResponse> queryHandler,
    ILogger<IQueryHandler<TQuery, TResponse>> logger) : IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling query handler for query {typeof(TQuery)}");
        try
        {
            return await queryHandler.HandleAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}