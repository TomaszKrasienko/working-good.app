using Shouldly;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

public sealed class TicketCreateTests
{
    [Fact]
    public void Create_ForValidArguments_ShouldReturnTicketWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var number = 1;
        var subject = "Test ticket";
        var content = "Test content";
        var createdAt = DateTime.Now;
        var createdBy = "joe.doe@email.pl";

        //act
        var result = Ticket.Create(id, number, subject, content, createdAt, createdBy);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Number.Value.ShouldBe(number);
        result.Subject.Value.ShouldBe(subject);
        result.Content.Value.ShouldBe(content);
        result.CreatedAt.Value.ShouldBe(createdAt);
        result.CreatedBy.Value.ShouldBe(createdBy);
    }

    [Fact]
    public void Create_GivenZeroNumber_ShouldThrowInvalidNumberException()
    {
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 0, "My subject", 
            "Test content", DateTime.Now, "joe.doe@email.pl"));
        
        //assert
        exception.ShouldBeOfType<InvalidNumberException>();
    }

    [Fact]
    public void Create_GivenEmptySubject_ShouldThrowEmptySubjectException()
    {        
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 1, string.Empty, 
            "Test content", DateTime.Now, "joe.doe@email.pl"));
        
        //assert
        exception.ShouldBeOfType<EmptySubjectException>();
    }
    
    [Fact]
    public void Create_GivenEmptyContent_ShouldThrowEmptyContentException()
    {        
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 1, "Test subject", 
            string.Empty, DateTime.Now, "joe.doe@email.pl"));
        
        //assert
        exception.ShouldBeOfType<EmptyContentException>();
    } 
}