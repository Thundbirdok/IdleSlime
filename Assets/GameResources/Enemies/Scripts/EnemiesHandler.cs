using UnityEngine;

namespace GameResources.Enemies.Scripts
{
    using System;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "EnemiesHandler", menuName = "Enemy/EnemiesHandler")]
    public class EnemiesHandler : ScriptableObject
    {
        public event Action OnUpdateObjects;
        
        [NonSerialized]
        private List<Enemy> _enemies = new List<Enemy>();
        public IReadOnlyList<Enemy> Enemies => _enemies;

        public void Add(Enemy enemy)
        {
            _enemies.Add(enemy);
            
            OnUpdateObjects?.Invoke();
        }

        public void Remove(Enemy enemy)
        {
            _enemies.Remove(enemy);
            
            OnUpdateObjects?.Invoke();
        }
    }
}
