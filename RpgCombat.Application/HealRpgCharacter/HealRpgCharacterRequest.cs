using MediatR;

namespace RpgCombat.Application.HealRpgCharacter
{
    public class HealRpgCharacterRequest : IRequest<HealRpgCharacterResponse>
    {
        public int CharacterId { get; set; }
        public int Heal { get; set; }
    }
}
