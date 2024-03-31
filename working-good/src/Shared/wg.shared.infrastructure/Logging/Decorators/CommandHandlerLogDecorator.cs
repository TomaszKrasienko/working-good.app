using Microsoft.Extensions.Logging;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.shared.infrastructure.Logging.Decorators;

internal sealed class CommandHandlerLogDecorator<T>(
    ICommandHandler<T> handler,
    ILogger<ICommandHandler<T>> logger) : ICommandHandler<T> where T : ICommand
{
    public async Task HandleAsync(T command, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling command handler for command: {typeof(T)}");
        try
        {
            await handler.HandleAsync(command, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}