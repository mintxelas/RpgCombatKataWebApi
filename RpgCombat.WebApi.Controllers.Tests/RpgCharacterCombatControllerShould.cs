using FluentAssertions;
using HashidsNet;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RpgCombat.Application.CreateRpgCharacter;
using RpgCombat.Application.DamageRpgCharacter;
using RpgCombat.Application.GetRpgCharacter;
using RpgCombat.Application.HealRpgCharacter;
using RpgCombat.WebApi.HttpModels;
using System.Threading.Tasks;
using Xunit;

namespace RpgCombat.WebApi.Controllers.Tests
{
    public class RpgCharacterCombatControllerShould
    {
        private const string StringId = "abc";
        private const int NumericId = 123;
        private readonly RpgCharacterCombatController controller;
        private readonly IMediator mediator;
        private readonly IHashids hashids;

        public RpgCharacterCombatControllerShould()
        {
            mediator = Substitute.For<IMediator>();
            hashids = Substitute.For<IHashids>();
            controller = new RpgCharacterCombatController(mediator, hashids);
        }

        [Fact]
        public async Task ReturnTheCharacterRequested()
        {
            var givenCharacter = new Domain.RpgCharacter();
            var expectedCharacter = new HttpModels.RpgCharacter { Level = givenCharacter.Level, Health = givenCharacter.Health };
            mediator.Send(Arg.Any<GetRpgCharacterRequest>()).Returns(new GetRpgCharacterResponse(givenCharacter));
            hashids.Decode(Arg.Any<string>()).Returns(new[]{NumericId});

            var actionResult = await controller.Get(StringId);

            await mediator.Received().Send(Arg.Is<GetRpgCharacterRequest>(req => req.Id == NumericId));
            hashids.Received().Decode(StringId);
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var correctResult = actionResult.Result as OkObjectResult;
            correctResult?.Value.Should().BeEquivalentTo(expectedCharacter);
        }

        [Fact]
        public async Task ReturnNewlyCreatedCharacterId()
        {
            mediator.Send(Arg.Any<CreateRpgCharacterRequest>()).Returns(new CreateRpgCharacterResponse(NumericId));
            hashids.Encode(NumericId).Returns(StringId);

            var actionResult = await controller.Post();

            await mediator.Received().Send(Arg.Any<CreateRpgCharacterRequest>());
            hashids.Received().Encode(NumericId);
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var correctResult = actionResult.Result as OkObjectResult;
            correctResult?.Value.Should().Be(StringId);
        }

        [Fact]
        public async Task DamageExistingCharacter()
        {
            mediator.Send(Arg.Any<DamageRpgCharacterRequest>()).Returns(new DamageRpgCharacterResponse());
            hashids.Decode(StringId).Returns(new[] {NumericId});

            var actionResult = await controller.Put(StringId, new Attack { Damage = 100 });

            await mediator.Received().Send(
                Arg.Is<DamageRpgCharacterRequest>(req => req.CharacterId == NumericId && req.Damage == 100));
            hashids.Received().Decode(StringId);
            actionResult.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task HealExistingCharacter()
        {
            mediator.Send(new HealRpgCharacterRequest {CharacterId = NumericId, Heal = 100})
                .Returns(new HealRpgCharacterResponse());
            hashids.Decode(StringId).Returns(new[] {NumericId});

            var actionResult = await controller.Put(StringId, new Heal() {Amount = 100});

            actionResult.Should().BeOfType<OkResult>();
            await mediator.Received()
                .Send(Arg.Is<HealRpgCharacterRequest>(req => req.CharacterId == NumericId && req.Heal == 100));
            hashids.Received().Decode(StringId);
        }
    }
}
