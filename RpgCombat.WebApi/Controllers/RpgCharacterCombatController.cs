using HashidsNet;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RpgCombat.Application.CreateRpgCharacter;
using RpgCombat.Application.DamageRpgCharacter;
using RpgCombat.Application.GetRpgCharacter;
using RpgCombat.Application.HealRpgCharacter;
using RpgCombat.WebApi.HttpModels;
using System.Linq;
using System.Threading.Tasks;

namespace RpgCombat.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RpgCharacterCombatController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHashids hashids;

        public RpgCharacterCombatController(IMediator mediator, IHashids hashids)
        {
            this.mediator = mediator;
            this.hashids = hashids;
        }

        [HttpGet("{characterId}")]
        public async Task<ActionResult<RpgCharacter>> Get([FromRoute]string characterId)
        {
            var characterResponse = await mediator.Send(new GetRpgCharacterRequest(ToDomainId(characterId)));
            return Ok(ToHttpModel(characterResponse.Character));
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post()
        {
            var characterCreationResponse = await mediator.Send(new CreateRpgCharacterRequest());
            return Ok(ToPublicId(characterCreationResponse.CharacterId));
        }

        [HttpPut("{characterId}/Attack")]
        public async Task<IActionResult> Put([FromRoute]string characterId, [FromBody]Attack attack)
        {
            _ = await mediator.Send(new DamageRpgCharacterRequest
            {
                CharacterId = ToDomainId(characterId),
                Damage = attack.Damage
            });
            return Ok();
        }

        [HttpPut("{characterId}/Heal")]
        public async Task<IActionResult> Put([FromRoute]string characterId, [FromBody]Heal heal)
        {
            _ = await mediator.Send(new HealRpgCharacterRequest
            {
                CharacterId = ToDomainId(characterId),
                Heal = heal.Amount
            });
            return Ok();
        }

        private string ToPublicId(int characterId) => hashids.Encode(characterId);

        private int ToDomainId(string characterId) => hashids.Decode(characterId).First();

        private RpgCharacter ToHttpModel(Domain.RpgCharacter character) =>
            new RpgCharacter { Health = character.Health, Level = character.Level };
    }
}
