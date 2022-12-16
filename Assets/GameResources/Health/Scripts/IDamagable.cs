namespace GameResources.Health.Scripts
{
    using System;
    using UnityEngine;

    public interface IDamagable
    {
        public event Action<IDamagable> OnDeath;

        public event Action OnAmountChange;
        
        public Vector3 Position { get; }
        
        public Vector3 HealthBarPosition { get; }
        
        public bool IsDead { get; }
        
        public int Health { get; }

        public int MaxHealth { get; }
        
        public void Damage(int value);

        public void Heal(int value);

        public void HealAll();
    }
}
