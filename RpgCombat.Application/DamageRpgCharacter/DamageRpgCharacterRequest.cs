using MediatR;

namespace RpgCombat.Application.DamageRpgCharacter
{
    public class DamageRpgCharacterRequest : IRequest<DamageRpgCharacterResponse>
    {
        public int CharacterId { get; set; }
        public int Damage { get; set; }
    }
}