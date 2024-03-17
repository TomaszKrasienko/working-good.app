using Shouldly;
using wg.modules.tickets.domain.Entities;
using Xunit;

namespace wg.modules.tickets.domain.tests;

public sealed class MessageCreateTests
{
    [Fact]
    public void Create_GivenAllValidArguments_ShouldReturnMessage()
    {
        //arrange
        var id = Guid.NewGuid();
        var sender = "joe@doe.pl";
        var subject = "Test subject";
        var content = "Test content";
        var createdAt = DateTime.Now;
        
        //act
        var result = Message.Create(id, sender, subject, content, createdAt);
        
        //assert
        result.ShouldNotBeNull();
        result.Sender.Value.ShouldBe(sender);
        result.Subject.Value.ShouldBe(subject);
        result.Content.Value.ShouldBe(content);
        result.CreatedAt.Value.ShouldBe(createdAt);
    }
    
    [Fact]
    public void Create_GivenEmptySender_ShouldThrowEmptySenderException()
    {
        //act
        var result = Message.Create(Guid.NewGuid(), "joe@doe.pl", "Test subject",
            "Test content", DateTime.Now);
        
        //assert
        result.ShouldNotBeNull();
        result.Sender.Value.ShouldBe(sender);
        result.Subject.Value.ShouldBe(subject);
        result.Content.Value.ShouldBe(content);
        result.CreatedAt.Value.ShouldBe(createdAt);
    }
}