using MediatR;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace RpgCombat.Application.GetRpgCharacter
{
    public class GetRpgCharacterHandler: IRequestHandler<GetRpgCharacterRequest, GetRpgCharacterResponse>
    {
        private IRpgCharacterRepository repository;

        public GetRpgCharacterHandler(IRpgCharacterRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GetRpgCharacterResponse> Handle(GetRpgCharacterRequest request, CancellationToken cancellationToken) 
            => new GetRpgCharacterResponse(await repository.GetById(request.Id));
    }
}