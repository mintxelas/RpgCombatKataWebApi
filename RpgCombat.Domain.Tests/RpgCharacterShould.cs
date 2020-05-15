using FluentAssertions;
using Xunit;

namespace RpgCombat.Domain.Tests
{
    public class RpgCharacterShould
    {
        private const int SomeId = 0;
        private const int SomeLevel = 1;

        [Fact]
        public void StartWithMaxHealthAndStartingLevel()
        {
            var character = new RpgCharacter();
            character.Health.Should().Be(RpgCharacter.MaxHealth);
            character.Level.Should().Be(RpgCharacter.StartingLevel);
        }

        [Theory]
        [InlineData(1000, 100, 900)]
        [InlineData(1000, 1100, 0)]
        [InlineData(0, 100, 0)]
        public void ReduceHealthByDamageWithAMinimumOfZero(int startingHealth, int dealtDamage, int expectedHealth)
        {
            var givenCharacter = new RpgCharacter(SomeId, startingHealth, SomeLevel);
            givenCharacter.Damage(dealtDamage);
            givenCharacter.Health.Should().Be(expectedHealth);
        }

        [Theory]
        [InlineData(RpgCharacter.MaxHealth, 100, RpgCharacter.MaxHealth)]
        [InlineData(500, 100, 600)]
        public void IncreaseHealthByHealedAmountWithoutSurpassingMaximum(int startingHealth, int healedAmount, int expectedHealth)
        {
            var givenCharacter = new RpgCharacter(SomeId, startingHealth, SomeLevel);
            givenCharacter.Heal(healedAmount);
            givenCharacter.Health.Should().Be(expectedHealth);
        }

        [Fact]
        public void DeadCharacterCannotBeHealed()
        {
            var givenCharacter = new RpgCharacter(SomeId, 0, SomeLevel);
            givenCharacter.Heal(100);
            givenCharacter.Health.Should().Be(0);
        }
        
    }
}