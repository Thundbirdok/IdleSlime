using UnityEngine;

namespace GameResources.Stats.Editor.Tests
{
    using FluentAssertions;
    using GameResources.Stats.Scripts;
    using NUnit.Framework;

    public class StatsHandlerTests : MonoBehaviour
    {
        [Test]
        public void WhenAddLevel_AndLevelWasZero_ThenLevelShouldBeOne()
        {
            //Arrange

            var stats = new[]
            {
                new Stat(100, 0), 
                new Stat(200, 0)
            };
            
            var statHandler = new StatHandler(stats, "", "health");

            //Act    
            
            statHandler.AddLevel(1);

            //Assert

            statHandler.CurrentLevel.Should().Be(1);
        }

        [Test]
        public void WhenAddLevel_AndLevelWasZero_ThenValueShouldBeEqualToLevelOneValue()
        {
            //Arrange
            
            var stats = new[]
            {
                new Stat(100, 0), 
                new Stat(200, 100)
            };
            
            var statHandler = new StatHandler(stats, "", "health");

            //Act    
            
            statHandler.AddLevel(1);

            //Assert

            statHandler.Value.Should().Be(stats[1].Value);
        }
        
        [Test]
        public void WhenInit_AndLevelWasZero_ThenValueShouldBeEqualToLevelZeroValue()
        {
            //Arrange
            
            var stats = new[]
            {
                new Stat(100, 0), 
                new Stat(200, 100)
            };
            
            var statHandler = new StatHandler(stats, "", "health");

            //Act

            //Assert

            statHandler.Value.Should().Be(stats[0].Value);
        }
        
        [Test]
        public void WhenAddLevel_AndLevelWasZero_ThenNextCostShouldBeEqualToLevelTwoCost()
        {
            //Arrange

            var stats = new[]
            {
                new Stat(100, 0), 
                new Stat(200, 100),
                new Stat(300, 200)
            };
            
            var statHandler = new StatHandler(stats, "", "health");

            //Act    
            
            statHandler.AddLevel(1);

            //Assert

            statHandler.NextLevelCost.Should().Be(stats[2].Cost);
        }
        
        [Test]
        public void WhenInit_AndLevelWasZero_ThenNextCostShouldBeEqualToLevelOneCost()
        {
            //Arrange

            var stats = new[]
            {
                new Stat(100, 0), 
                new Stat(200, 100),
                new Stat(300, 200)
            };
            
            var statHandler = new StatHandler(stats, "", "health");

            //Act

            //Assert

            statHandler.NextLevelCost.Should().Be(stats[1].Cost);
        }
    }
}
