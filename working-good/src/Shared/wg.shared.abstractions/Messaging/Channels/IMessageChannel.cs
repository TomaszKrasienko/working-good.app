using System.Threading.Channels;

namespace wg.shared.abstractions.Messaging.Channels;

public interface IMessageChannel
{
    ChannelReader<IMessage> Reader { get; }
    ChannelWriter<IMessage> Writer { get; }
}