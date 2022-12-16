using UnityEngine;

namespace GameResources.Health.Scripts
{
    using System;

    [Serializable]
    public class Health
    {
        public event Action OnValueChange; 
        public event Action OnDeath;

        public bool IsDead => _amount <= 0;
        
        [SerializeField]
        private int maxHealth = 100;

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

                _amount = value;

                if (_amount < 0)
                {
                    _amount = 0;
                }

                OnValueChange?.Invoke();
                
                if (_amount == 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        public void Damage(int value)
        {
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

        public void HealAll() => Amount = maxHealth;

        public void SetWithoutNotify(int value)
        {
            if (value < 0)
            {
                Debug.Log("Can not be below zero");
                
                return;
            }
            
            _amount = value;
        }

        public void HealAllWithoutNotify() => _amount = maxHealth;
    }
}
