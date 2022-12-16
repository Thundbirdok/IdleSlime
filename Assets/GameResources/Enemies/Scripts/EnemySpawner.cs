using UnityEngine;

namespace GameResources.Enemies.Scripts
{
    using System.Collections;
    using System.Collections.Generic;
    using GameResources.Health.Scripts;
    using GameResources.Slime.Scripts;
    using UnityEngine.Pool;

    public class EnemySpawner : MonoBehaviour
    {
        private List<Enemy> _enemies = new List<Enemy>();
        public IReadOnlyList<Enemy> Enemies => _enemies;
        
        [SerializeField]
        private Slime slime;
        
        [SerializeField]
        private Enemy enemyPrefab;

        [SerializeField]
        private Transform enemyContainer;

        [SerializeField]
        private float spawnRate = 6;
        
        [SerializeField]
        private Transform[] enemySpawnPositions;

        private Coroutine _coroutine;

        private ObjectPool<Enemy> _pool;

        private void Awake() => InitEnemyPool();

        private void OnEnable()
        {
            StartSpawnCoroutine();
        }

        private void OnDisable()
        {
            StopSpawnCoroutine();
            
            _pool.Clear();
        }

        private void OnDestroy() => DestroyEnemyPool();

        private void InitEnemyPool()
        {
            _pool = new ObjectPool<Enemy>
            (
                CreateFunc,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
        }

        private void DestroyEnemyPool() => _pool = null;

        private void ActionOnDestroy(Enemy enemy) => Destroy(enemy.gameObject);

        private void ActionOnRelease(Enemy enemy)
        {
            enemy.OnDeath -= OnEnemyDeath;
            
            enemy.gameObject.SetActive(false);

            _enemies.Remove(enemy);
        }

        private void ActionOnGet(Enemy enemy)
        {
            enemy.Init(slime);
            
            var index = Random.Range(0, enemySpawnPositions.Length);
            var spawnPoint = enemySpawnPositions[index];
            
            enemy.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            
            enemy.OnDeath += OnEnemyDeath;
            
            enemy.gameObject.SetActive(true);
            
            _enemies.Add(enemy);
        }

        private Enemy CreateFunc()
        {
            var enemy = Instantiate(enemyPrefab, enemyContainer);

            return enemy;
        }

        private void StartSpawnCoroutine()
        {
            _coroutine = StartCoroutine(AutoSpawn());
        }

        private void StopSpawnCoroutine()
        {
            if (_coroutine == null)
            {
                return;
            }

            StopCoroutine(_coroutine);

            _coroutine = null;
        }

        private IEnumerator AutoSpawn()
        {
            var delay = new WaitForSeconds(60 / spawnRate);
            
            while (enabled)
            {
                Spawn();
                
                yield return delay;
            }
        }

        private void Spawn() => _pool.Get();

        private void OnEnemyDeath(IDamagable enemy) => _pool.Release(enemy as Enemy);
    }
}
