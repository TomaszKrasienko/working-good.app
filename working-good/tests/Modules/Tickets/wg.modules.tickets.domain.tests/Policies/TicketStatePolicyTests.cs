using Shouldly;
using wg.modules.tickets.domain.Policies;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using Xunit;

namespace wg.modules.tickets.domain.tests.Policies;

public sealed class TicketStatePolicyTests
{
    [Theory]
    [InlineData("New")]
    [InlineData("Open")]
    [InlineData("InProgress")]
    [InlineData("WaitingForResponse")]
    public void CanChangeState_GivenStateForChange_ShouldReturnTrue(string state)
    {
        //arrange
        var policy = TicketStatePolicy.Create();
        
        //act
        var result = policy.CanChangeState(new Status(state, DateTime.Now));
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData("Cancelled")]
    [InlineData("Done")]
    public void CanChangeState_NotGivenStateForChange_ShouldReturnFalse(string state)
    {
        //arrange
        var policy = TicketStatePolicy.Create();
        
        //act
        var result = policy.CanChangeState(new Status(state, DateTime.Now));
        
        //assert
        result.ShouldBeFalse();
    }
}