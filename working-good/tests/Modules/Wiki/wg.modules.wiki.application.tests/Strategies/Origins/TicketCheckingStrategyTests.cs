using NSubstitute;
using Shouldly;
using wg.modules.wiki.application.Clients.Tickets;
using wg.modules.wiki.application.Clients.Tickets.DTOs;
using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.application.Strategies.Origins;
using Xunit;

namespace wg.modules.wiki.application.tests.Strategies.Origins;

public sealed class TicketCheckingStrategyTests
{
    [Fact]
    public async Task IsExists_GivenExistingTicket_ShouldReturnTrue()
    {
        //arrange
        var originId = Guid.NewGuid();
        _ticketsApiClient
            .IsTicketExistsAsync(new TicketIdDto(originId))
            .Returns(new IsTicketExistsDto(true));
        
        //act
        var result = await _strategy.IsExists(originId.ToString());

        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public async Task IsExists_GivenNotExistingTicket_ShouldReturnFalse()
    {
        //arrange
        var originId = Guid.NewGuid();
        _ticketsApiClient
            .IsTicketExistsAsync(new TicketIdDto(originId))
            .Returns(new IsTicketExistsDto(false));
        
        //act
        var result = await _strategy.IsExists(originId.ToString());

        //assert
        result.ShouldBeFalse();
    }
    
    [Fact]
    public async Task IsExists_GivenInvalidGuid_ShouldThrowOriginIdIsInvalidException()
    {
        //arrange
        var originId = "invalid_guid";
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _strategy.IsExists(originId));
        
        //assert
        exception.ShouldBeOfType<OriginIdIsInvalidException>();
    }
    
    #region arrange
    private readonly ITicketsApiClient _ticketsApiClient;
    private readonly IOriginCheckingStrategy _strategy;
    
    public TicketCheckingStrategyTests()
    {
        _ticketsApiClient = Substitute.For<ITicketsApiClient>();
        _strategy = new TicketCheckingStrategy(_ticketsApiClient);
    }
    #endregion
}