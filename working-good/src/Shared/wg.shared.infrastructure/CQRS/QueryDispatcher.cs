using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.shared.infrastructure.CQRS;

internal sealed class QueryDispatcher(IServiceProvider serviceProvider)
    : IQueryDispatcher
{
    public async Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return await ((Task<TResult>) handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))?
            .Invoke(handler, new object[]{query, cancellationToken}))!;
    }
}