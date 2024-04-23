using wg.modules.messages.core.Services.Commands;

namespace wg.modules.messages.core.Services.Abstractions;

public interface IMessageService
{
    Task CreateMessage(CreateMessage command);
}