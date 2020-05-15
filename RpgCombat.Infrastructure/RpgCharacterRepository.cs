using System.Linq;
using RpgCombat.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgCombat.Infrastructure.EntityFramework;

namespace RpgCombat.Infrastructure
{
    public class RpgCharacterRepository : IRpgCharacterRepository
    {
        private readonly RpgCombatKataDbContext context;

        public RpgCharacterRepository(RpgCombatKataDbContext context)
        {
            this.context = context;
        }

        public async Task<RpgCharacter> Create()
        {
            var newCharacter = new Domain.RpgCharacter();
            var newRecord = ToRecord(newCharacter);
            var storedEntity = await context.RpgCharacters.AddAsync(newRecord);
            await context.SaveChangesAsync();
            newCharacter = FromRecord(storedEntity.Entity);
            return newCharacter;
        }

        public Task<RpgCharacter> GetById(int characterId)
        {
            var record = context.RpgCharacters.Single(c => c.Id == characterId);
            return Task.FromResult(new RpgCharacter(record.Id, record.Health, record.Level));
        }

        public async Task Save(RpgCharacter character)
        {
            var record = await context.RpgCharacters.SingleAsync(c => c.Id == character.Id);
            record.Health = character.Health;
            record.Level = character.Level;
            await context.SaveChangesAsync();
        }

        private RpgCharacter FromRecord(RpgCharacterRecord record)
        {
            return new RpgCharacter(record.Id, record.Health, record.Level);
        }

        private RpgCharacterRecord ToRecord(RpgCharacter character)
        {
            return new RpgCharacterRecord()
            {
                Id = character.Id,
                Health = character.Health,
                Level = character.Level
            };
        }
    }
}