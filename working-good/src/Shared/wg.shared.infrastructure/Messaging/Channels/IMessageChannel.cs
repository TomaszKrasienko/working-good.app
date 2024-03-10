using System.Threading.Channels;
using wg.shared.abstractions.Messaging;

namespace wg.shared.infrastructure.Messaging.Channels;

public interface IMessageChannel
{
    ChannelReader<IMessage> Reader { get; }
    ChannelWriter<IMessage> Writer { get; }
}