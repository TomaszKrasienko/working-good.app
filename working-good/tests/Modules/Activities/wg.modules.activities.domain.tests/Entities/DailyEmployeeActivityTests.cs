using Shouldly;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Exceptions;
using wg.tests.shared.Factories.Activities;
using Xunit;

namespace wg.modules.activities.domain.tests.Entities;

public sealed class DailyEmployeeActivityTests
{
    [Fact]
    public void AddPaidActivity_GivenNotCollisionActivity_ShouldReturnAddActivity()
    {
         //arrange
         var dailyEmployeeActivity = DailyEmployeeActivityFactory.Get();
         
         //act
         dailyEmployeeActivity.AddPaidActivity(Guid.NewGuid(), "My test content",
             Guid.NewGuid(), DateTime.Now, DateTime.Now.AddHours(1));
         
         //assert
         dailyEmployeeActivity.Activities.ShouldNotBeEmpty();
         var activity = dailyEmployeeActivity.Activities.Single();
         activity.ShouldBeOfType<PaidActivity>();
    }

    [Fact]
    public void AddPaidActivity_GivenCollisionActivity_ShouldThrowActivityCollisionTimeException()
    {
        //arrange
        var dailyEmployeeActivity = DailyEmployeeActivityFactory.Get();
        var paidActivity = ActivityFactory.GetPaidActivity(
            new DateTime(2024, 4, 26, 14, 30, 0),
            new DateTime(2024, 4, 26, 17, 00, 0));
        dailyEmployeeActivity.AddPaidActivity(paidActivity.Id, paidActivity.Content, paidActivity.TicketId,
            paidActivity.ActivityTime.TimeFrom, paidActivity.ActivityTime.TimeTo);

        var timeFrom = new DateTime(2024, 4, 26, 13, 30, 0);
        var timeTo = new DateTime(2024, 4, 26, 14, 35, 0);
        
        //act
        var exception = Record.Exception(() => dailyEmployeeActivity.AddPaidActivity(Guid.NewGuid(), "My test content",
            Guid.NewGuid(), timeFrom, timeTo));
         
        //assert
        exception.ShouldBeOfType<ActivityCollisionTimeException>();
    }
    
    [Fact]
    public void AddInternalActivity_GivenNotCollisionActivity_ShouldReturnAddActivity()
    {
        //arrange
        var dailyEmployeeActivity = DailyEmployeeActivityFactory.Get();
         
        //act
        dailyEmployeeActivity.AddInternalActivity(Guid.NewGuid(), "My test content",
            Guid.NewGuid(), DateTime.Now, DateTime.Now.AddHours(1));
         
        //assert
        dailyEmployeeActivity.Activities.ShouldNotBeEmpty();
        var activity = dailyEmployeeActivity.Activities.Single();
        activity.ShouldBeOfType<PaidActivity>();
    }

    [Fact]
    public void AddInternalActivity_GivenCollisionActivity_ShouldThrowActivityCollisionTimeException()
    {
        //arrange
        var dailyEmployeeActivity = DailyEmployeeActivityFactory.Get();
        var paidActivity = ActivityFactory.GetPaidActivity(
            new DateTime(2024, 4, 26, 14, 30, 0),
            new DateTime(2024, 4, 26, 17, 00, 0));
        dailyEmployeeActivity.AddInternalActivity(paidActivity.Id, paidActivity.Content, paidActivity.TicketId,
            paidActivity.ActivityTime.TimeFrom, paidActivity.ActivityTime.TimeTo);

        var timeFrom = new DateTime(2024, 4, 26, 13, 30, 0);
        var timeTo = new DateTime(2024, 4, 26, 14, 35, 0);
        
        //act
        var exception = Record.Exception(() => dailyEmployeeActivity.AddPaidActivity(Guid.NewGuid(), "My test content",
            Guid.NewGuid(), timeFrom, timeTo));
         
        //assert
        exception.ShouldBeOfType<ActivityCollisionTimeException>();
    }
}