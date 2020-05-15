namespace RpgCombat.Application.GetRpgCharacter
{
    public class GetRpgCharacterResponse
    {
        public Domain.RpgCharacter Character { get; }

        public GetRpgCharacterResponse(Domain.RpgCharacter character)
        {
            Character = character;
        }
    }
}