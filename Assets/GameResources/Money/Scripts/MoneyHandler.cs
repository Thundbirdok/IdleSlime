namespace GameResources.Money.Scripts
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "MoneyHandler", menuName = "Economics/MoneyHandler")]
    public class MoneyHandler : ScriptableObject
    {
        public event Action<int> OnAmountChange;

        public event Action OnNotEnough;
        
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
            }
        }

        public void Add(int value)
        {
            Amount += value;
        }

        public void Spend(int value)
        {
            if (Amount < value)
            {
                OnNotEnough?.Invoke();
                
                return;
            }
            
            Amount -= value;
        }
    }
}
