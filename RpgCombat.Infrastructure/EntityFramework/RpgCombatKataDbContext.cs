using Microsoft.EntityFrameworkCore;

namespace RpgCombat.Infrastructure.EntityFramework
{
    public class RpgCombatKataDbContext: DbContext
    {
        public DbSet<RpgCharacterRecord> RpgCharacters { get; set; }

        public RpgCombatKataDbContext(DbContextOptions<RpgCombatKataDbContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RpgCharacterRecord>().HasKey(x => x.Id);
        }
    }
}