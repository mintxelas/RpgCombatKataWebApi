using MediatR;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace RpgCombat.Application.DamageRpgCharacter
{
    public class DamageRpgCharacterHandler: IRequestHandler<DamageRpgCharacterRequest, DamageRpgCharacterResponse>
    {
        private IRpgCharacterRepository repository;

        public DamageRpgCharacterHandler(IRpgCharacterRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DamageRpgCharacterResponse> Handle(DamageRpgCharacterRequest request, CancellationToken cancellationToken)
        {
            var character = await repository.GetById(request.CharacterId);
            character.Damage(request.Damage);
            await repository.Save(character);
            return new DamageRpgCharacterResponse();
        }
    }
}