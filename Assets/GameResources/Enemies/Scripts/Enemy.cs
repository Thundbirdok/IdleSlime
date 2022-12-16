using UnityEngine;

namespace GameResources.Enemies.Scripts
{
    using System;
    using System.Collections;
    using GameResources.Health.Scripts;
    using GameResources.Slime.Scripts;

    public class Enemy : MonoBehaviour, IDamagable
    {
        public event Action<IDamagable> OnDeath;
        public event Action OnAmountChange;

        public Vector3 Position => transform.position;
        public Vector3 HealthBarPosition => healthBarPosition.position;

        public bool IsDead => health.IsDead;
        public int Health => health.Amount;
        public int MaxHealth => health.MaxAmount;
        
        public Vector3 PositionAfterSecond => Vector3.MoveTowards
        (
            transform.position,
            _destinationPoint,
            speed
        );
        
        [SerializeField]
        private Health health;

        [SerializeField]
        private Transform healthBarPosition;
        
        [SerializeField]
        private float speed = 2.5f;

        [SerializeField]
        private float stopMoveDistance = 1.5f;

        [SerializeField]
        private float attackRate = 12;
        
        [SerializeField]
        private int damage = 10;
        
        private Slime _target;

        private Vector3 _destinationPoint;

        private float _distanceToTarget;

        private Coroutine _coroutine;
        
        public void Damage(int value) => health.Damage(value);

        public void Heal(int value) => health.Heal(value);

        public void HealAll() => health.HealAll();

        private void OnEnable()
        {
            health.Init(this);
            
            health.OnDeath += InvokeOnDeath;
            health.OnAmountChange += InvokeOnAmountChange;
            
            StartAutoAttackCoroutine();
        }

        private void OnDisable()
        {
            health.Dispose();
            
            health.OnDeath -= InvokeOnDeath;
            health.OnAmountChange -= InvokeOnAmountChange;
            
            StopAutoAttackCoroutine();
        }

        private void FixedUpdate()
        {
            Move();
            GetDistanceToTarget();
        }

        private void StartAutoAttackCoroutine()
        {
            _coroutine = StartCoroutine(AutoAttack());
        }

        private void StopAutoAttackCoroutine()
        {
            if (_coroutine == null)
            {
                return;
            }

            StopCoroutine(_coroutine);

            _coroutine = null;
        }

        public void Init(Slime target)
        {
            _target = target;

            var targetPosition = _target.transform.position;
            var reverseDirection = (transform.position - targetPosition);
            _destinationPoint = targetPosition + reverseDirection.normalized * stopMoveDistance;
            
            health.HealAllWithoutNotify();
        }

        private void GetDistanceToTarget()
        {
            _distanceToTarget = Vector3.Distance
            (
                transform.position, 
                _target.transform.position
            );
        }
        
        private void Move()
        {
            if (_distanceToTarget <= stopMoveDistance)
            {
                return;
            }

            transform.position = Vector3.MoveTowards
            (
                transform.position,
                _destinationPoint,
                speed * Time.fixedDeltaTime
            );
        }

        private IEnumerator AutoAttack()
        {
            var delay = new WaitForSeconds(60 / attackRate);
            
            while (enabled)
            {
                if (_target == null)
                {
                    yield return null;

                    continue;
                }
                
                if (_distanceToTarget > stopMoveDistance)
                {
                    yield return null;

                    continue;
                }
                
                Attack();

                yield return delay;
            }
        }

        private void Attack() => _target.Damage(damage);

        private void InvokeOnDeath() => OnDeath?.Invoke(this);
        private void InvokeOnAmountChange() => OnAmountChange?.Invoke();
    }
}
