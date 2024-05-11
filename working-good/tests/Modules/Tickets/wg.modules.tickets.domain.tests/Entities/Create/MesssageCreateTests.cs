using Shouldly;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

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
        var exception = Record.Exception(() => Message.Create(Guid.NewGuid(), string.Empty, "Test subject",
            "Test content", DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<EmptySenderException>();
    }
    
    [Fact]
    public void Create_GivenEmptySubject_ShouldThrowEmptySubjectException()
    {   
        //act
        var exception = Record.Exception(() => Message.Create(Guid.NewGuid(), "joe@doe.pl", string.Empty,
            "Test content", DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<EmptySubjectException>();
    }
    
    [Fact]
    public void Create_GivenEmptyContent_ShouldThrowEmptyContentException()
    {   
        //act
        var exception = Record.Exception(() => Message.Create(Guid.NewGuid(), "joe@doe.pl", "Test subject",
            string.Empty, DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<EmptyContentException>();
    }
}