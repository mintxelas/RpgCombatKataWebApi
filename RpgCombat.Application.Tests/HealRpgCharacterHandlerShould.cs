using FluentAssertions;
using NSubstitute;
using RpgCombat.Application.HealRpgCharacter;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgCombat.Application.Tests
{
    public class HealRpgCharacterHandlerShould
    {
        private const int CharacterId = 123;
        private const int Heal = 456;

        [Fact]
        public async Task ApplyHealingToGivenCharacter()
        {
            var request = new HealRpgCharacterRequest { CharacterId = CharacterId, Heal = Heal };
            var repository = Substitute.For<IRpgCharacterRepository>();
            var givenCharacter = Substitute.For<RpgCharacter>(CharacterId, 1000, 1);
            repository.GetById(CharacterId).Returns(givenCharacter);
            var handler = new HealRpgCharacterHandler(repository);

            var response = await handler.Handle(request, CancellationToken.None);

            response.Should().NotBeNull();
            givenCharacter.Received().Heal(Arg.Is<int>(d => d == Heal));
            await repository.Received().Save(Arg.Is<RpgCharacter>(c => c.Id == CharacterId));
        }
    }
}
