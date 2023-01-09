using System;
using System.Linq;
using UnityEngine;

namespace GameResources.Stats.Scripts
{
    using JsonSave = GameResources.JsonSave.JsonSave;

    [Serializable]
    public class StatHandler : IStatHandler
    {
        public int NextLevelCost
        {
            get
            {
                var nextLevel = CurrentLevel + 1;
                
                if (nextLevel >= Levels.Length)
                {
                    return Levels.Last().Cost + (nextLevel - Levels.Length + 1) * 10;
                }
                
                return Levels[nextLevel].Cost;
            }
        }

        public int Value
        {
            get
            {
                if (CurrentLevel >= Levels.Length)
                {
                    return Levels.Last().Value + (CurrentLevel - Levels.Length + 1) * 10;
                }
                
                return Levels[CurrentLevel].Value;
            }
        }

        [field: SerializeField]
        public string Description { get; private set; } = "";

        [SerializeField]
        private string name;
        
        [SerializeField]
        private Stat[] levels;
        public Stat[] Levels => levels;
        
        private string FileName => name + ".json";

        private const string KEY = "Level";
        
        private JsonSave _save;
        
        [field: NonSerialized] 
        public int CurrentLevel { get; private set; }

        public StatHandler() {}

        public StatHandler(Stat[] levels, string description, string name = "")
        {
            this.levels = levels;
            Description = description;
            this.name = name;
        }
        
        public void Load()
        {
            _save = new JsonSave(FileName, KEY);
            
            CurrentLevel = _save.Load();
        }

        public void Save()
        {
            _save.Save(CurrentLevel);
        }

        public void AddLevel(int value)
        {
            CurrentLevel += value;
        }
    }
}
