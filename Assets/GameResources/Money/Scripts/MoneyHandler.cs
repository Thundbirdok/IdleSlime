namespace GameResources.Money.Scripts
{
    using System;
    using GameResources.JsonSave;
    using UnityEngine;

    [CreateAssetMenu(fileName = "MoneyHandler", menuName = "Economics/MoneyHandler")]
    public class MoneyHandler : SavableScriptableObject
    {
        public event Action<int> OnAmountChange;

        public event Action OnNotEnough;
        
        [NonSerialized]
        private int _amount;

        private JsonSave _save;

        private const string FILE_NAME = "EconomyResources.json";

        private const string KEY = "Money";

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

        public override void Load()
        {
            _save = new JsonSave(FILE_NAME, KEY);
            
            Amount = _save.Load();
        }

        public override void Save() => _save.Save(_amount);

        public void Add(int value) => Amount += value;

        public bool Spend(int value)
        {
            if (Amount < value)
            {
                OnNotEnough?.Invoke();
                
                return false;
            }
            
            Amount -= value;

            return true;
        }
    }
}
