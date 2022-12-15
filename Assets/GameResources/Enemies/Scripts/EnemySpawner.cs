using UnityEngine;

namespace GameResources.Enemies.Scripts
{
    using System.Collections;
    using GameResources.Health.Scripts;
    using GameResources.Slime.Scripts;
    using UnityEngine.Pool;

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Slime slime;
        
        [SerializeField]
        private Enemy enemyPrefab;

        [SerializeField]
        private Transform enemyContainer;

        [SerializeField]
        private float spawnRate = 12;
        
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
        }

        private void ActionOnGet(Enemy enemy)
        {
            var spawnPoint = enemySpawnPositions[UnityEngine.Random.Range(0, enemySpawnPositions.Length)];
            
            enemy.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            
            enemy.OnDeath += OnEnemyDeath;
            
            enemy.gameObject.SetActive(true);
        }

        private Enemy CreateFunc()
        {
            var enemy = Instantiate(enemyPrefab, enemyContainer);

            enemy.Init(slime);

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
