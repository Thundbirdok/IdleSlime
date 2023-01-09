namespace GameResources.Health.Editor.Tests
{
    using FluentAssertions;
    using GameResources.Health.Scripts;
    using GameResources.Stats.Scripts;
    using NUnit.Framework;

    public class HealthTests
    {
        [Test]
        public void WhenAddingHealth_AndHealthZero_ThenHealthShouldBeEqualToAdded()
        {
            //Arrange    

            var levels = new []
            {
                new Stat(100, 0)
            };

            var stat = new StatHandler(levels, "", "health");
            
            var health = new Health(stat);
            
            //Act    

            health.Heal(10);

            //Assert
            
            health.Amount.Should().Be(10);
        }

        [Test]
        public void WhenAddingHealth_AndHealthIsMax_ThenHealthShouldBeEqualMax()
        {
            //Arrange    

            var levels = new []
            {
                new Stat(100, 0)
            };

            var stat = new StatHandler(levels, "", "health");
            
            var health = new Health(stat);

            //Act    

            health.HealAll();
            health.Heal(10);

            //Assert
            
            health.Amount.Should().Be(health.MaxAmount);
        }

        [Test]
        public void WhenDamage_AndHealthWasEqualToDamage_ThenHealthShouldBeZero()
        {
            //Arrange    

            var levels = new []
            {
                new Stat(100, 0)
            };

            var stat = new StatHandler(levels, "", "health");
            
            var health = new Health(stat);
            
            //Act    

            health.HealAll();
            health.Damage(100);
            
            //Assert

            health.Amount.Should().Be(0);
        }
        
        [Test]
        public void WhenDamage_AndHealthWasLessThanDamage_ThenHealthShouldBeZero()
        {
            //Arrange    

            var levels = new []
            {
                new Stat(100, 0)
            };

            var stat = new StatHandler(levels, "", "health");
            
            var health = new Health(stat);
            
            //Act    

            health.HealAll();
            health.Damage(150);
            
            //Assert

            health.Amount.Should().Be(0);
        }
        
        [Test]
        public void WhenDamage_AndHealthWasEqualToDamage_ThenInvokeDeathAction()
        {
            //Arrange    

            var levels = new []
            {
                new Stat(100, 0)
            };

            var stat = new StatHandler(levels, "", "health");
            
            var health = new Health(stat);
            
            //Act    

            var onDeathCount = 0;
            
            health.OnDeath += OnDeath;
            
            health.HealAll();
            health.Damage(100);
            
            health.OnDeath -= OnDeath;
            
            //Assert

            onDeathCount.Should().Be(1);
            
            void OnDeath()
            {
                ++onDeathCount;
            }
        }
    }
}
