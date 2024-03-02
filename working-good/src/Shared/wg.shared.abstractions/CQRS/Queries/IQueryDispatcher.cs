namespace wg.shared.abstractions.CQRS.Queries;

public interface IQueryDispatcher
{
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
}