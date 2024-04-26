using Shouldly;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Policy;
using wg.tests.shared.Factories.Activities;
using wg.tests.shared.Helpers;
using Xunit;

namespace wg.modules.activities.domain.tests.Policies;

public sealed class CollisionTimePolicyTests
{
    [Fact]
    public void HasCollision_GivenNoCollisionActivity_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 15, 00, 00),
                new DateTime(2024, 4, 26, 16, 00, 00)),
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 17, 00, 00),
                new DateTime(2024, 4, 26, 18, 00, 00))
        };

        var policy = CollisionTimePolicy.GetInstance();

        var timeFrom = new DateTime(2024, 4, 26, 16, 00, 00);
        var timeTo = new DateTime(2024, 4, 26, 17, 00, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, timeTo);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void HasCollision_GivenCollisionTimeForAlreadyTimeWithTimeToNull_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 15, 00, 00),
                null),
        };

        var policy = CollisionTimePolicy.GetInstance();

        var timeFrom = new DateTime(2024, 4, 26, 16, 00, 00);
        var timeTo = new DateTime(2024, 4, 26, 17, 00, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, timeTo);
        
        //assert
        result.ShouldBeFalse();
    }
}