using FluentAssertions;
using NSubstitute;
using RpgCombat.Application.CreateRpgCharacter;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgCombat.Application.Tests
{
    public class CreateRpgCharacterHandlerShould
    {
        private readonly IRpgCharacterRepository repository;
        private readonly CreateRpgCharacterHandler handler;

        public CreateRpgCharacterHandlerShould()
        {
            repository = Substitute.For<IRpgCharacterRepository>();
            handler = new CreateRpgCharacterHandler(repository);
        }

        [Fact]
        public async Task RetrieveNewCharacterIdFromRepository()
        {
            var expectedCharacter = new Domain.RpgCharacter();
            repository.Create().Returns(expectedCharacter);

            var response = await handler.Handle(new CreateRpgCharacterRequest(), CancellationToken.None);

            response.CharacterId.Should().Be(expectedCharacter.Id);
        }
    }
}