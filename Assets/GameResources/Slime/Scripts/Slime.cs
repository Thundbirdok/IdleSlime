using UnityEngine;

namespace GameResources.Slime.Scripts
{
    using System;
    using GameResources.Health.Scripts;

    public class Slime : MonoBehaviour, IDamagable
    {
        public event Action<IDamagable> OnDeath;
        
        public bool IsDead => health.IsDead;

        public int Health => health.Amount;

        [SerializeField]
        private Health health;
        
        public void Damage(int value) => health.Damage(value);

        public void Heal(int value) => health.Heal(value);

        public void HealAll() => health.HealAll();

        private void OnEnable() => health.OnDeath += InvokeOnDeath;

        private void OnDisable() => health.OnDeath -= InvokeOnDeath;
        
        private void InvokeOnDeath() => OnDeath?.Invoke(this);
    }
}
