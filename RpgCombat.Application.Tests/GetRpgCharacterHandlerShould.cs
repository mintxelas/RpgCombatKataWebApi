using FluentAssertions;
using NSubstitute;
using RpgCombat.Application.GetRpgCharacter;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgCombat.Application.Tests
{
    public class GetRpgCharacterHandlerShould
    {
        private const int SomeId = 123;
        private const int SomeHealth = 456;
        private const int SomeLevel = 7;

        public GetRpgCharacterHandlerShould()
        {
        }

        [Fact]
        public async Task ReturnTheCharacterCorrespondingToTheRequestedId()
        {
            var repository = Substitute.For<IRpgCharacterRepository>();
            var expectedCharacter = new RpgCharacter(SomeId, SomeHealth, SomeLevel);
            repository.GetById(SomeId).Returns(expectedCharacter);
            var handler = new GetRpgCharacterHandler(repository);

            var response = await handler.Handle(new GetRpgCharacterRequest(SomeId), CancellationToken.None);

            response.Character.Should().Be(expectedCharacter);
        }
    }
}