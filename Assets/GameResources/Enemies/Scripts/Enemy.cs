using UnityEngine;

namespace GameResources.Enemies.Scripts
{
    using System;
    using GameResources.Health.Scripts;
    using GameResources.Slime.Scripts;

    public class Enemy : MonoBehaviour, IDamagable
    {
        public event Action<IDamagable> OnDeath;
        
        public bool IsDead => health.IsDead;
        public int Health => health.Amount;

        [SerializeField]
        private Health health;

        [SerializeField]
        private float speed = 0.5f;

        [SerializeField]
        private float stopMoveDistance = 1.5f;
        
        private Slime _target;
        
        public void Damage(int value) => health.Damage(value);

        public void Heal(int value) => health.Heal(value);

        public void HealAll() => health.HealAll();

        private void OnEnable() => health.OnDeath += InvokeOnDeath;

        private void OnDisable() => health.OnDeath -= InvokeOnDeath;

        private void FixedUpdate() => Move();

        public void Init(Slime target)
        {
            _target = target;

            health.HealAllWithoutNotify();
        }

        private void Move()
        {
            var distanceToTarget = Vector3.Distance
            (
                transform.position, 
                _target.transform.position
            );

            if (distanceToTarget <= stopMoveDistance)
            {
                return;
            }

            transform.position = Vector3.MoveTowards
            (
                transform.position,
                _target.transform.position,
                speed * Time.fixedDeltaTime
            );
        }

        private void InvokeOnDeath() => OnDeath?.Invoke(this);
    }
}
