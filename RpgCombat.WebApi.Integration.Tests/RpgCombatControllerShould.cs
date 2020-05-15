using FluentAssertions;
using HashidsNet;
using MediatR;
using NSubstitute;
using RpgCombat.Application.CreateRpgCharacter;
using RpgCombat.Application.DamageRpgCharacter;
using RpgCombat.Application.GetRpgCharacter;
using RpgCombat.WebApi.HttpModels;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RpgCombat.Application.HealRpgCharacter;
using Xunit;

namespace RpgCombat.WebApi.Tests
{
    public class RpgCombatControllerShould: IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const int SomeNumericId = 123;
        private const string SomeHashedId = "someId";
        private const int SomeDamage = 456;
        private const int SomeHealing = 321;
        private const int SomeHealth = 123;
        private const int SomeLevel = 4;
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly HttpClient client;

        public RpgCombatControllerShould(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            factory.Hashids = Substitute.For<IHashids>();
            factory.CreateRpgCharacterHandler =
                Substitute.For<IRequestHandler<CreateRpgCharacterRequest, CreateRpgCharacterResponse>>();
            factory.DamageRpgCharacterHandler =
                Substitute.For<IRequestHandler<DamageRpgCharacterRequest, DamageRpgCharacterResponse>>();
            factory.HealRpgCharacterHandler =
                Substitute.For<IRequestHandler<HealRpgCharacterRequest, HealRpgCharacterResponse>>();
            factory.GetRpgCharacterHandler = Substitute.For<IRequestHandler<GetRpgCharacterRequest, GetRpgCharacterResponse>>();
            client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateNewCharacter()
        {
            factory.CreateRpgCharacterHandler.Handle(Arg.Any<CreateRpgCharacterRequest>(),
                    Arg.Any<CancellationToken>())
                .Returns(new CreateRpgCharacterResponse(SomeNumericId));
            factory.Hashids.Encode(SomeNumericId).Returns(SomeHashedId);

            var characterHttpResponse = await client.PostAsync("RpgCharacterCombat", null);

            characterHttpResponse.EnsureSuccessStatusCode();
            var characterId = await characterHttpResponse.Content.ReadAsStringAsync();
            characterId.Should().Be(SomeHashedId);
        }

        [Fact]
        public async Task DamageCharacter()
        {
            factory.DamageRpgCharacterHandler.Handle(Arg.Any<DamageRpgCharacterRequest>(), Arg.Any<CancellationToken>())
                .Returns(new DamageRpgCharacterResponse());
            var givenAttack = new Attack { Damage = SomeDamage };
            var content = new StringContent(JsonSerializer.Serialize(givenAttack), System.Text.Encoding.UTF8, "application/json");
            factory.Hashids.Decode(SomeHashedId).Returns(new[] { SomeNumericId });
            factory.Hashids.Encode(SomeNumericId).Returns(SomeHashedId);

            var characterHttpResponse = await client.PutAsync($"RpgCharacterCombat/{SomeHashedId}/Attack", content);

            characterHttpResponse.EnsureSuccessStatusCode();
            await factory.DamageRpgCharacterHandler.Received()
                .Handle(Arg.Is<DamageRpgCharacterRequest>(req => req.Damage == SomeDamage && req.CharacterId == SomeNumericId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HealCharacter()
        {
            factory.HealRpgCharacterHandler.Handle(Arg.Any<HealRpgCharacterRequest>(), Arg.Any<CancellationToken>())
                .Returns(new HealRpgCharacterResponse());
            var givenHeal = new Heal { Amount = SomeHealing };
            var content = new StringContent(JsonSerializer.Serialize(givenHeal), System.Text.Encoding.UTF8, "application/json");
            factory.Hashids.Decode(SomeHashedId).Returns(new[] { SomeNumericId });
            factory.Hashids.Encode(SomeNumericId).Returns(SomeHashedId);

            var characterHttpResponse = await client.PutAsync($"RpgCharacterCombat/{SomeHashedId}/Heal", content);

            characterHttpResponse.EnsureSuccessStatusCode();
            await factory.HealRpgCharacterHandler.Received()
                .Handle(Arg.Is<HealRpgCharacterRequest>(req => req.Heal == SomeHealing && req.CharacterId == SomeNumericId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RetrieveCharacter()
        {
            var givenRpgCharacter = GivenRpgCharacter(SomeNumericId);
            factory.GetRpgCharacterHandler.Handle(Arg.Any<GetRpgCharacterRequest>(), Arg.Any<CancellationToken>())
                .Returns(new GetRpgCharacterResponse(givenRpgCharacter));
            factory.Hashids.Decode(SomeHashedId).Returns(new[] { SomeNumericId });
            factory.Hashids.Encode(SomeNumericId).Returns(SomeHashedId);

            var characterHttpResponse = await client.GetAsync($"RpgCharacterCombat/{SomeHashedId}");

            characterHttpResponse.EnsureSuccessStatusCode();
            await factory.GetRpgCharacterHandler.Received()
                .Handle(Arg.Is<GetRpgCharacterRequest>(req => req.Id == givenRpgCharacter.Id),
                    Arg.Any<CancellationToken>());
        }

        private Domain.RpgCharacter GivenRpgCharacter(int someNumericId) => new Domain.RpgCharacter(someNumericId, SomeHealth, SomeLevel);

        private string GivenCharacterId(int id) => SomeHashedId;
    }
}