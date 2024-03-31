using Microsoft.Extensions.Logging;
using wg.shared.abstractions.Events;

namespace wg.shared.infrastructure.Logging.Decorators;

[Decorator]
public class EventHandlerLogDecorator<T>(
    IEventHandler<T> handler,
    ILogger<IEventHandler<T>> logger) : IEventHandler<T> where T : class, IEvent
{
    public async Task HandleAsync(T @event)
    {
        logger.LogInformation($"Handling event handler for event: {typeof(T)}");
        try
        {
            await handler.HandleAsync(@event);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}