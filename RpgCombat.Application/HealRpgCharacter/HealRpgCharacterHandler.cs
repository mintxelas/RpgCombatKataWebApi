using MediatR;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace RpgCombat.Application.HealRpgCharacter
{
    public class HealRpgCharacterHandler : IRequestHandler<HealRpgCharacterRequest, HealRpgCharacterResponse>
    {
        private readonly IRpgCharacterRepository repository;

        public HealRpgCharacterHandler(IRpgCharacterRepository repository)
        {
            this.repository = repository;
        }

        public async Task<HealRpgCharacterResponse> Handle(HealRpgCharacterRequest request, CancellationToken cancellationToken)
        {
            var character = await repository.GetById(request.CharacterId);
            character.Heal(request.Heal);
            await repository.Save(character);
            return new HealRpgCharacterResponse();
        }
    }
}
