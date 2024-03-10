using System.Threading.Channels;
using wg.shared.abstractions.Messaging;

namespace wg.shared.infrastructure.Messaging.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IMessage> _messagesChannel = Channel.CreateUnbounded<IMessage>();
    public ChannelReader<IMessage> Reader => _messagesChannel.Reader;
    public ChannelWriter<IMessage> Writer => _messagesChannel.Writer;
}