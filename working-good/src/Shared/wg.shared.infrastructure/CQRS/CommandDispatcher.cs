using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.shared.infrastructure.CQRS;

internal sealed class CommandDispatcher(IServiceProvider serviceProvider) 
    : ICommandDispatcher
{
    
    
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : class, ICommand
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }
}