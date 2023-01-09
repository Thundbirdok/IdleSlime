namespace GameResources.Stats.Scripts
{
    using System;
    using System.Linq;
    using GameResources.JsonSave;
    using UnityEngine;

    [CreateAssetMenu(fileName = "StateHandler", menuName = "Stats/Handler")]
    public class StatHandlerSo : SavableScriptableObject, IStatHandler
    {
        [SerializeField]
        private StatHandler statHandler;

        public override void Load() => statHandler.Load();

        public override void Save() => statHandler.Save();

        public int NextLevelCost => statHandler.NextLevelCost;
        public int Value => statHandler.Value;
        public string Description => statHandler.Description;
        public Stat[] Levels => statHandler.Levels;
        
        public void AddLevel(int value) => statHandler.AddLevel(value);
    }
}
