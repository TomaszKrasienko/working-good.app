using Shouldly;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Policy;
using wg.tests.shared.Factories.Activities;
using Xunit;

namespace wg.modules.activities.domain.tests.Policies;

public sealed class CollisionTimePolicyTests
{
    [Fact]
    public void HasCollision_GivenNoCollisionActivityWithBetweenTime_ShouldReturnFalse()
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
        result.ShouldBeFalse();
    }
    
    [Fact]
    public void HasCollision_GivenNoCollisionActivityWithTimeToAsNull_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 15, 00, 00),
                new DateTime(2024, 4, 26, 16, 00, 00))
        };

        var policy = CollisionTimePolicy.GetInstance();

        var timeFrom = new DateTime(2024, 4, 26, 16, 00, 00);
    
        //act
        var result = policy.HasCollision(activities, timeFrom, null);
    
        //assert
        result.ShouldBeFalse();
    }
    
    [Fact]
    public void HasCollision_GivenNoCollisionActivityWithExistingActivityWithNullTimeTo_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 15, 00, 00),null)
        };

        var policy = CollisionTimePolicy.GetInstance();

        var timeFrom = new DateTime(2024, 4, 26, 14, 00, 00);
        var timeTo = new DateTime(2024, 4, 26, 15, 00, 00);
    
        //act
        var result = policy.HasCollision(activities, timeFrom, timeTo);
    
        //assert
        result.ShouldBeFalse();
    }
    
    [Fact]
    public void HasCollision_GivenCollisionNullTimeToActivityWithTimeFromEarlierThanExistingTimeTo_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 14, 00, 00),
                new DateTime(2024, 4, 26, 15, 15, 00, 00)),
        };

        var policy = CollisionTimePolicy.GetInstance();

        var timeFrom = new DateTime(2024, 4, 26, 15, 0, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, null);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void HasCollision_GivenCollisionNullTimeToActivityWithTwoNullTimeTo_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 13, 00, 00),
                null)
        };

        var policy = CollisionTimePolicy.GetInstance();

        var timeFrom = new DateTime(2024, 4, 26, 15, 0, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, null);
        
        //assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void HasCollision_GivenCollisionNullTimeToActivityWithActivityAfter_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 16, 0,0), 
                null)
        };
        var policy = CollisionTimePolicy.GetInstance();
        var timeFrom = new DateTime(2024, 4, 26, 15, 00, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, null);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void HasCollision_GivenCollisionActivityWithExistingEarlierActivityWithNullTimeTo_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 14, 0,0), 
                null)
        };
        var policy = CollisionTimePolicy.GetInstance();
        var timeFrom = new DateTime(2024, 4, 26, 15, 00, 00);
        var timeTo = new DateTime(2024, 4, 26, 16, 00, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, timeTo);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void HasCollision_GivenCollisionActivityWithExistingEarlierActivity_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 14, 0,0), 
                new DateTime(2024, 4, 26, 15, 5,0))
        };
        var policy = CollisionTimePolicy.GetInstance();
        var timeFrom = new DateTime(2024, 4, 26, 15, 00, 00);
        var timeTo = new DateTime(2024, 4, 26, 16, 00, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, timeTo);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void HasCollision_GivenCollisionActivityWithExistingLaterActivity_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 15, 30,0), 
                new DateTime(2024, 4, 26, 17, 00,0))
        };
        var policy = CollisionTimePolicy.GetInstance();
        var timeFrom = new DateTime(2024, 4, 26, 15, 00, 00);
        var timeTo = new DateTime(2024, 4, 26, 16, 00, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, timeTo);
        
        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void HasCollision_GivenCollisionActivityWithExistingCoveringActivity_ShouldReturnFalse()
    {
        //arrange
        var activities = new List<Activity>()
        {
            ActivityFactory.GetRandom(
                new DateTime(2024, 4, 26, 14, 30,0), 
                new DateTime(2024, 4, 26, 17, 00,0))
        };
        var policy = CollisionTimePolicy.GetInstance();
        var timeFrom = new DateTime(2024, 4, 26, 15, 00, 00);
        var timeTo = new DateTime(2024, 4, 26, 16, 00, 00);
        
        //act
        var result = policy.HasCollision(activities, timeFrom, timeTo);
        
        //assert
        result.ShouldBeTrue();
    }
}