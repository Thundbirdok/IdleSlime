using UnityEngine;

namespace GameResources.Health.Scripts
{
    using System;
    using System.Collections;

    [Serializable]
    public class Health
    {
        public event Action<int> OnAmountChange;

        public event Action OnDeath;

        [NonSerialized]
        private int _amount;
        public int Amount
        {
            get => _amount;

            private set
            {
                if (_amount == value)
                {
                    return;
                }

                var previous = _amount;
                
                _amount = value;

                if (_amount < 0)
                {
                    _amount = 0;
                }

                OnAmountChange?.Invoke(_amount - previous);
                
                if (_amount == 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        public bool IsDead => _amount <= 0;

        public bool IsInvincible => _invincibleSeconds > 0;

        [field: SerializeField]
        public int MaxAmount { get; private set; } = 100;

        private MonoBehaviour _monoBehaviour;

        private Coroutine _coroutine;

        [NonSerialized]
        private float _invincibleSeconds;

        public void Init(MonoBehaviour gameObject)
        {
            _monoBehaviour = gameObject;
        }

        public void Dispose()
        {
            StopCountDown();
        }
        
        public void Damage(int value)
        {
            if (IsInvincible)
            {
                return;
            }
            
            if (value < 0)
            {
                Debug.LogError("Use Heal Instead");
                
                return;
            }
            
            Amount -= value;
        }

        public void Heal(int value)
        {
            if (value < 0)
            {
                Debug.LogError("Use Damage Instead");
                
                return;
            }
            
            Amount += value;
        }

        public void HealAll() => Amount = MaxAmount;

        public void AddInvincible(float seconds)
        {
            _invincibleSeconds += seconds;

            StartInvincibleCountDownCoroutine();
        }

        public void SetWithoutNotify(int value)
        {
            if (value < 0)
            {
                Debug.Log("Can not be below zero");
                
                return;
            }
            
            _amount = value;
        }

        public void HealAllWithoutNotify() => _amount = MaxAmount;

        private void StartInvincibleCountDownCoroutine()
        {
            if (_coroutine == null)
            {
                _coroutine = _monoBehaviour.StartCoroutine(InvincibleCountdown());
            }
        }

        private void StopCountDown()
        {
            if (_coroutine == null)
            {
                return;
            }

            _monoBehaviour.StopCoroutine(_coroutine);

            _coroutine = null;
        }
        
        private IEnumerator InvincibleCountdown()
        {
            while (_invincibleSeconds > 0)
            {
                _invincibleSeconds -= Time.fixedDeltaTime;

                yield return null;
            }

            _invincibleSeconds = 0;
        }
    }
}
