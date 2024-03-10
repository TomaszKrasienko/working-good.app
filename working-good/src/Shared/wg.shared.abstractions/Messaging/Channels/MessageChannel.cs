using System.Threading.Channels;

namespace wg.shared.abstractions.Messaging.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IMessage> _messagesChannel = Channel.CreateUnbounded<IMessage>();
    public ChannelReader<IMessage> Reader => _messagesChannel.Reader;
    public ChannelWriter<IMessage> Writer => _messagesChannel.Writer;
}