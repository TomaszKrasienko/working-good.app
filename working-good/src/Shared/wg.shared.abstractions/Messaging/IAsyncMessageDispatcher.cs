namespace wg.shared.abstractions.Messaging;

public interface IAsyncMessageDispatcher
{
    Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;
}