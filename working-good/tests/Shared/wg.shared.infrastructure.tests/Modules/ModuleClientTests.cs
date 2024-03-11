using NSubstitute;
using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Modules;
using wg.shared.infrastructure.Modules.Abstractions;
using wg.shared.infrastructure.Modules.Models;
using Xunit;

namespace wg.shared.infrastructure.tests.Modules;

public sealed class ModuleClientTests
{
    [Fact]
    public async Task PublishAsync_GivenMessage_ShouldInvokeActions()
    {
        //arrange
        var actionMoq = Substitute.For<Func<object, Task>>();
        var messageToSend = new MessageToSend()
        {
            Value = "my_value"
        };

        var messageToReceive = new MessageToReceive()
        {
            Value = messageToSend.Value
        };

        _moduleRegistry
            .GetBroadcastRegistrations(messageToSend.GetType().Name)
            .Returns(new[]
            {
                new ModuleBroadcastRegistration(typeof(MessageToReceive), actionMoq)
            }); 

        _moduleTypesTranslator
            .TranslateType(messageToSend, typeof(MessageToReceive))
            .Returns(messageToReceive);
        
        //act
        await _moduleClient.PublishAsync(messageToSend);
        
        //assert
        await actionMoq
            .Received(1)
            .Invoke(messageToReceive);
    }
    
    #region arrange
    private readonly IModuleRegistry _moduleRegistry;
    private readonly IModuleTypesTranslator _moduleTypesTranslator;
    private readonly IModuleClient _moduleClient;

    public ModuleClientTests()
    {
        _moduleRegistry = Substitute.For<IModuleRegistry>();
        _moduleTypesTranslator = Substitute.For<IModuleTypesTranslator>();
        _moduleClient = new ModuleClient(_moduleRegistry, _moduleTypesTranslator);
    }
    #endregion
}

public class MessageToSend
{
    public string Value { get; set; }
}

public class MessageToReceive
{
    public string Value { get; set; }
}