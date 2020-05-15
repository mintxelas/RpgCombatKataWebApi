using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RpgCombat.Domain;
using RpgCombat.Infrastructure.EntityFramework;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RpgCombat.Infrastructure.Tests
{
    public sealed class RpgCharacterRepositoryShould: IDisposable
    {
        private readonly RpgCharacterRepository repository;
        private readonly RpgCombatKataDbContext dbContext;
        private const int SomeOtherHealth = 123;
        private const int SomeOtherLevel = 3;
        private const int SomeHealth = 456;
        private const int SomeLevel = 5;

        public RpgCharacterRepositoryShould()
        {
            dbContext = new RpgCombatKataDbContext(
                new DbContextOptionsBuilder<RpgCombatKataDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);
            repository = new RpgCharacterRepository(dbContext);
        }

        [Fact]
        public async Task CreateANewCharacter()
        {
            var actualCharacter = await repository.Create();
            actualCharacter.Should().NotBeNull();
        }

        [Fact]
        public async Task AssignDifferentIdsToDifferentCharacters()
        {
            var aCharacter = await repository.Create();
            var anotherCharacter = await repository.Create();
            aCharacter.Id.Should().NotBe(anotherCharacter.Id);
        }

        [Fact]
        public async Task ReturnTheRequestedCharacterById()
        {
            var givenCharacter = await GivenAnExistingCharacter();
            var actualCharacter = await repository.GetById(givenCharacter.Id);
            actualCharacter.Should().BeEquivalentTo(givenCharacter);
        }

        [Fact]
        public async Task PersistTheGivenCharacterChanges()
        {
            var givenRecord = await GivenAnExistingCharacter();
            var changedCharacter = new RpgCharacter(givenRecord.Id, SomeOtherHealth, SomeOtherLevel);
            
            await repository.Save(changedCharacter);

            await ThenGivenCharacterIsPersistedWithADifferentHealth(changedCharacter);
        }

        private async Task ThenGivenCharacterIsPersistedWithADifferentHealth(RpgCharacter character)
        {
            var record = await dbContext.RpgCharacters.SingleOrDefaultAsync(c => c.Id == character.Id 
                && c.Health == character.Health && c.Level == character.Level);
            record.Should().BeEquivalentTo(character);
        }

        private async Task<RpgCharacterRecord> GivenAnExistingCharacter()
        {
            var record = await dbContext.RpgCharacters.AddAsync(new RpgCharacterRecord()
            {
                Health = SomeHealth,
                Level = SomeLevel
            });
            await dbContext.SaveChangesAsync();
            return record.Entity;
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}