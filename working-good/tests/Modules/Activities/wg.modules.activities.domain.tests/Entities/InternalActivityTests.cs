using Shouldly;
using wg.modules.activities.domain.Entities;
using wg.tests.shared.Factories.Activities;
using Xunit;

namespace wg.modules.activities.domain.tests.Entities;

public sealed class InternalActivityTests
{
    [Fact]
    public void ChangeType_GivenInternalActivity_ShouldReturnPaidActivity()
    {
        //arrange
        var internalActivity = ActivityFactory.GetInternalActivity(DateTime.Now, null);
        
        //act
        var result = internalActivity.ChangeType();
        
        //assert
        result.ShouldBeOfType<PaidActivity>();
        result.Id.ShouldBe(internalActivity.Id);
        result.Content.ShouldBe(internalActivity.Content);
        result.TicketId.ShouldBe(internalActivity.TicketId);
        result.ActivityTime.ShouldBe(internalActivity.ActivityTime);
    }
}