namespace GameResources.Health.Scripts
{
    using System;

    public interface IDamagable
    {
        public event Action<IDamagable> OnDeath;

        public bool IsDead { get; }
        
        public int Health { get; }

        public void Damage(int value);

        public void Heal(int value);

        public void HealAll();
    }
}
