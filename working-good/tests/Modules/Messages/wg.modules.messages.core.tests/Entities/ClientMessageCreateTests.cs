using Shouldly;
using wg.modules.messages.core.Entities;
using Xunit;

namespace wg.modules.messages.core.tests.Entities;

public sealed class ClientMessageCreateTests
{
    [Fact]
    public void Create_GivenSubjectStartedFromNumber_ShouldReturnMessageWithFilledNumber()
    {
        //arrange
        var number = 14;
        var subject = $"#{number} - Test subject";
        var content = "Test content";
        var sender = "test@test.pl";
        var createdAt = DateTime.Now;
        
        //act
        var message = ClientMessage.Create(subject, content, sender, createdAt);
        
        //assert
        message.Id.Value.ShouldNotBe(Guid.Empty);
        message.Subject.ShouldBe(subject);
        message.Content.ShouldBe(content);
        message.Sender.ShouldBe(sender);
        message.CreatedAt.ShouldBe(createdAt);
        message.Number.ShouldBe(number);
    }

    [Fact]
    public void Create_GivenSubjectWithoutNumber_ShouldReturnMessageWithNullNumber()
    {        
        //arrange
        var subject = "#Test subject";
        var content = "Test content";
        var sender = "test@test.pl";
        var createdAt = DateTime.Now;
        
        //act
        var message = ClientMessage.Create(subject, content, sender, createdAt);
        
        //assert
        message.Id.Value.ShouldNotBe(Guid.Empty);
        message.Subject.ShouldBe(subject);
        message.Content.ShouldBe(content);
        message.Sender.ShouldBe(sender);
        message.CreatedAt.ShouldBe(createdAt);
        message.Number.ShouldBeNull();
    }
}