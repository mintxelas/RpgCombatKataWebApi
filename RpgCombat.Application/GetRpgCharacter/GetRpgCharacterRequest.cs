using MediatR;

namespace RpgCombat.Application.GetRpgCharacter
{
    public class GetRpgCharacterRequest : IRequest<GetRpgCharacterResponse>
    {
        public int Id { get; }

        public GetRpgCharacterRequest(int id)
        {
            Id = id;
        }
    }
}