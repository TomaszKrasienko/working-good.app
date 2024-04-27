using Shouldly;
using wg.modules.activities.domain.Entities;
using wg.tests.shared.Factories.Activities;
using Xunit;

namespace wg.modules.activities.domain.tests.Entities;

public sealed class PaidActivityTests
{
    [Fact]
    public void ChangeType_GivenPaidActivity_ShouldReturnInternalActivity()
    {
        //arrange
        var paidActivity = ActivityFactory.GetPaidActivity(DateTime.Now, null);
        
        //act
        var result = paidActivity.ChangeType();
        
        //assert
        result.ShouldBeOfType<InternalActivity>();
        result.Id.ShouldBe(paidActivity.Id);
        result.Content.ShouldBe(paidActivity.Content);
        result.TicketId.ShouldBe(paidActivity.TicketId);
        result.ActivityTime.ShouldBe(paidActivity.ActivityTime);
    }
}