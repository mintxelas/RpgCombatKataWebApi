using System;

namespace RpgCombat.Domain
{
    public class RpgCharacter
    {
        public const int StartingLevel = 1;
        public const int MaxHealth = 1000;
        public int Health { get; private set; }
        public int Level { get; }
        public int Id { get; }

        public RpgCharacter()
        {
            Id = 0;
            Health = MaxHealth;
            Level = StartingLevel;
        }

        public RpgCharacter(int id, int health, int level)
        {
            Id = id;
            Health = health;
            Level = level;
        }

        public virtual void Damage(int damage)
        {
            Health = Math.Max(0, Health - damage);
        }

        public virtual void Heal(int healing)
        {
            if (Health == 0) return;
            Health = Math.Min(MaxHealth, Health + healing);
        }
    }
}