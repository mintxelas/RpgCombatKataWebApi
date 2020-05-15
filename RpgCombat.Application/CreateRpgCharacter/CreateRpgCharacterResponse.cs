using RpgCombat.Domain;

namespace RpgCombat.Application.CreateRpgCharacter
{
    public class CreateRpgCharacterResponse
    {
        public int CharacterId { get; }

        public CreateRpgCharacterResponse(int id)
        {
            CharacterId = id;
        }
    }
}