namespace GameResources.Stats.Scripts
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "StateHandler", menuName = "Stats/Handler")]
    public class StatHandler : ScriptableObject
    {
        public int NextLevelCost
        {
            get
            {
                var nextLevel = _currentLevel + 1;
                
                if (nextLevel >= levels.Length)
                {
                    return levels[^1].Cost + (nextLevel - levels.Length + 1) * 10;
                }
                
                return levels[nextLevel].Cost;
            }
        }

        public int Value
        {
            get
            {
                if (_currentLevel >= levels.Length)
                {
                    return levels[^1].Value + (_currentLevel - levels.Length + 1) * 10;
                }
                
                return levels[_currentLevel].Value;
            }
        }

        [field: SerializeField]
        public string Description { get; private set; } = "";

        [SerializeField]
        private Stat[] levels;

        [NonSerialized]
        private int _currentLevel;

        public void AddLevel(int value)
        {
            _currentLevel += value;
        }
        
        [Serializable]
        private class Stat
        {
            public int Value;
            public int Cost;
        }
    }
}
