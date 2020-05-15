using System.Threading.Tasks;

namespace RpgCombat.Domain
{
    public interface IRpgCharacterRepository
    {
        Task<RpgCharacter> Create();
        Task Save(RpgCharacter character);
        Task<RpgCharacter> GetById(int characterId);
    }
}