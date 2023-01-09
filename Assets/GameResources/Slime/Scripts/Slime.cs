using UnityEngine;

namespace GameResources.Slime.Scripts
{
    using System;
    using GameResources.Health.Scripts;

    public class Slime : MonoBehaviour, IDamagable
    {
        public event Action<IDamagable> OnDeath;
        public event Action<IDamagable, int> OnAmountChange;

        public Vector3 Position => transform.position;
        
        public Vector3 HealthBarPosition => healthBarPosition.position;
        
        public bool IsDead => health.IsDead;

        public int Health => health.Amount;

        public int MaxHealth => health.MaxAmount;
        
        [SerializeField]
        private Health health;

        [SerializeField]
        private Transform healthBarPosition;
        
        public void Damage(int value) => health.Damage(value);

        public void Heal(int value) => health.Heal(value);

        public void HealAll() => health.HealAll();

        public void AddInvincible(float seconds) => health.AddInvincible(seconds);
        
        private void OnEnable()
        {
            health.Init(this);
            
            health.HealAll();
            
            health.OnDeath += InvokeOnDeath;
            health.OnAmountChange += InvokeOnAmountChange;
        }

        private void OnDisable()
        {
            health.Dispose();
            
            health.OnDeath -= InvokeOnDeath;
            health.OnAmountChange -= InvokeOnAmountChange;
        }

        private void InvokeOnDeath() => OnDeath?.Invoke(this);

        private void InvokeOnAmountChange(int value) => OnAmountChange?.Invoke(this, value);
    }
}
