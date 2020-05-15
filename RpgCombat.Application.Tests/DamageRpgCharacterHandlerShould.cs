using FluentAssertions;
using NSubstitute;
using RpgCombat.Application.DamageRpgCharacter;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgCombat.Application.Tests
{
    public class DamageRpgCharacterHandlerShould
    {
        private const int CharacterId = 123;
        private const int Damage = 456;

        [Fact]
        public async Task ApplyDamageToGivenCharacter()
        {
            var request = new DamageRpgCharacterRequest { CharacterId = CharacterId, Damage = Damage };
            var repository = Substitute.For<IRpgCharacterRepository>();
            var givenCharacter = Substitute.For<RpgCharacter>(CharacterId, 1000, 1);
            repository.GetById(CharacterId).Returns(givenCharacter);
            var handler = new DamageRpgCharacterHandler(repository);

            var response = await handler.Handle(request, CancellationToken.None);
            
            response.Should().NotBeNull();
            givenCharacter.Received().Damage(Arg.Is<int>(d => d == Damage));
            await repository.Received().Save(Arg.Is<RpgCharacter>(c => c.Id == CharacterId));
        }
    }
}
