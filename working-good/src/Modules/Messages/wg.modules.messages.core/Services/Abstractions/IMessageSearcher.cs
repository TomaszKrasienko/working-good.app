namespace wg.modules.messages.core.Services.Abstractions;

public interface IMessageSearcher
{
    Task SearchEmails(CancellationToken cancellationToken);
}