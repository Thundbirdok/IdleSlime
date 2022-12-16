namespace GameResources.Stats.Scripts
{
    using System;
    using GameResources.JsonSave;
    using UnityEngine;

    [CreateAssetMenu(fileName = "StateHandler", menuName = "Stats/Handler")]
    public class StatHandler : SavableScriptableObject
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

        private string FileName => name + ".json";

        private const string KEY = "Level";
        
        private JsonSave _save;
        
        [NonSerialized]
        private int _currentLevel;

        public override void Load()
        {
            _save = new JsonSave(FileName, KEY);
            
            _currentLevel = _save.Load();
        }

        public override void Save()
        {
            _save.Save(_currentLevel);
        }

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
