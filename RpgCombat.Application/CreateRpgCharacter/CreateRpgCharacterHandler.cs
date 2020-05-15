using MediatR;
using RpgCombat.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace RpgCombat.Application.CreateRpgCharacter
{
    public class CreateRpgCharacterHandler: IRequestHandler<CreateRpgCharacterRequest, CreateRpgCharacterResponse>
    {
        private readonly IRpgCharacterRepository repository;

        public CreateRpgCharacterHandler(IRpgCharacterRepository repository)
        {
            this.repository = repository;
        }

        public async Task<CreateRpgCharacterResponse> Handle(CreateRpgCharacterRequest request, CancellationToken cancellationToken)
        {
            var character = await repository.Create();
            return new CreateRpgCharacterResponse(character.Id);
        }
    }
}