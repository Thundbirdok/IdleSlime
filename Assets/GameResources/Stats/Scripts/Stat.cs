using System;
using UnityEngine;

namespace GameResources.Stats.Scripts
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField]
        public int Value { get; private set; }
        
        [field: SerializeField]
        public int Cost  { get; private set; }

        public Stat() {}

        public Stat(int value, int cost)
        {
            Value = value;
            Cost = cost;
        }
    }
}
