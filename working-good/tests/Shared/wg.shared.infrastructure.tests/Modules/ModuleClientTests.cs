using NSubstitute;
using Shouldly;
using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Modules;
using wg.shared.infrastructure.Modules.Abstractions;
using wg.shared.infrastructure.Modules.Models;
using wg.tests.shared.Models;
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

    [Fact]
    public async Task SendAsync_GivenExistingPath_ShouldReturnResultFromAction()
    {
        //arrange
        string path = "test/path";
        var messageToReceive = new MessageToReceive()
        {
            Value = "test value"
        };
        var response = new MessageToSend()
        {
            Value = "test value"
        };
        _moduleRegistry
            .GetRequestRegistration(path)
            .Returns(new ModuleRequestRegistration(typeof(MessageToSend), typeof(MessageToReceive), x
                => Task.FromResult((object)messageToReceive)));
        _moduleTypesTranslator
            .TranslateType<MessageToSend>(messageToReceive)
            .Returns(response);
        
        //act
        var result = await _moduleClient.SendAsync<MessageToSend>(path, messageToReceive);
        
        //assert
        result.ShouldBe(response);
    }
    
    [Fact]
    public async Task SendAsync_GivenNotExistingPath_ShouldThrowInvalidOperationException()
    {
        //arrange
        string path = "test/path";
        _moduleRegistry
            .GetRequestRegistration(path);
        
        //act
        var exception = await Record.ExceptionAsync(async () 
            => await _moduleClient.SendAsync<MessageToSend>(path, new MessageToReceive()));
        
        //assert
        exception.ShouldBeOfType<InvalidOperationException>();
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

