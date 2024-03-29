using Shouldly;
using wg.modules.tickets.application.DTOs;
using wg.tests.shared.Factories.Tickets;
using wg.modules.tickets.infrastructure.Queries.Mappers;
using Xunit;

namespace wg.modules.tickets.infrastructure.tests.Queries.Mappers;

public class ExtensionsTest
{
    [Fact]
    public void AsDto_GivenMessage_ShouldReturnMessageDto()
    {
        //arrange
        var message = MessagesFactory.Get().Single();
        
        //act
        var result = message.AsDto();
        
        //assert
        result.ShouldBeOfType<MessageDto>();
        result.Id.ShouldBe(message.Id.Value);
        result.Sender.ShouldBe(message.Sender.Value);
        result.Subject.ShouldBe(message.Subject.Value);
        result.Content.ShouldBe(message.Content.Value);
        result.CreatedAt.ShouldBe(message.CreatedAt.Value);
    }
}