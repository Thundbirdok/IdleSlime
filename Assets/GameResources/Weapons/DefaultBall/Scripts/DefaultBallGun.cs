using UnityEngine;

namespace GameResources.Weapons.DefaultBall.Scripts
{
    using System.Collections;
    using GameResources.Enemies.Scripts;
    using GameResources.Stats.Scripts;
    using PathCreation;
    using UnityEngine.Pool;

    public class DefaultBallGun : MonoBehaviour
    {
        [SerializeField]
        private Collider ownerCollider;
        
        [SerializeField]
        private StatHandler fireRate;

        [SerializeField] 
        private DefaultGunProjectile projectilePrefab;

        [SerializeField]
        private Transform projectileFirePoint;
        
        [SerializeField]
        private Transform projectilesContainer;

        [SerializeField]
        private EnemiesHandler enemiesHandler;

        private const float ADDITIONAL_HEIGHT_WITH_DISTANCE = 0.2f;
        private const float MIDDLE_1_POINT_DISTANCE = 0.333f;
        private const float MIDDLE_2_POINT_DISTANCE = 0.666f;
        
        private Coroutine _coroutine;

        private ObjectPool<DefaultGunProjectile> _projectilePool;

        private void Awake() => InitProjectilePool();

        private void OnEnable() => StartAutoFireCoroutine();

        private void OnDisable()
        {
            StopAutoFireCoroutine();

            _projectilePool.Clear();
        }

        private void OnDestroy() => DestroyProjectilePool();

        private void InitProjectilePool()
        {
            _projectilePool = new ObjectPool<DefaultGunProjectile>
            (
                CreateFunc,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
        }

        private void DestroyProjectilePool()
        {
            _projectilePool = null;
        }

        private void StartAutoFireCoroutine()
        {
            _coroutine = StartCoroutine(AutoFire());
        }

        private void StopAutoFireCoroutine()
        {
            if (_coroutine == null)
            {
                return;
            }

            StopCoroutine(_coroutine);

            _coroutine = null;
        }

        private static void ActionOnDestroy(DefaultGunProjectile projectile)
        {
            Destroy(projectile.gameObject);
        }

        private void ActionOnRelease(DefaultGunProjectile projectile)
        {
            projectile.OnDeath -= OnProjectileDeath;
            
            projectile.gameObject.SetActive(false);
        }

        private void ActionOnGet(DefaultGunProjectile projectile)
        {
            projectile.transform.position = projectileFirePoint.position;
            
            projectile.OnDeath += OnProjectileDeath;
            
            projectile.gameObject.SetActive(true);
        }

        private DefaultGunProjectile CreateFunc()
        {
            var projectile = Instantiate(projectilePrefab, projectilesContainer);

            Physics.IgnoreCollision
            (
                projectile.GetComponent<Collider>(),
                ownerCollider
            );

            return projectile;
        }

        private IEnumerator AutoFire()
        {
            var delay = new WaitForSeconds(60f / fireRate.Value);
            
            while (enabled)
            {
                Fire();
                
                yield return delay;
            }
        }

        private void Fire()
        {
            if (TryGetClosestEnemy(out var closestEnemy))
            {
                return;
            }
            
            var projectile = _projectilePool.Get();

            var impactPosition = closestEnemy.PositionAfterSecond;
            
            var path = GetProjectilePath(impactPosition);

            projectile.Fire(path);
        }

        private bool TryGetClosestEnemy(out Enemy closestEnemy)
        {
            var minDistance = float.MaxValue;

            closestEnemy = null;

            foreach (var enemy in enemiesHandler.Enemies)
            {
                var distance = Vector3.Distance
                (
                    enemy.transform.position,
                    transform.position
                );

                if (minDistance <= distance)
                {
                    continue;
                }

                minDistance = distance;
                closestEnemy = enemy;
            }

            return closestEnemy == null;
        }

        private VertexPath GetProjectilePath(Vector3 target)
        {
            var startPosition = projectileFirePoint.position;
            var direction = target - startPosition;

            var middle1 = startPosition + direction * MIDDLE_1_POINT_DISTANCE;
            var middle2 = startPosition + direction * MIDDLE_2_POINT_DISTANCE;

            var verticalDistance = target.y - startPosition.y;
            var middleHeight = Mathf.Max(verticalDistance, startPosition.y);
            
            middleHeight += direction.magnitude * ADDITIONAL_HEIGHT_WITH_DISTANCE;
            
            middle1.y = middleHeight;
            middle2.y = middleHeight;
            
            var points = new[]
            {
                startPosition,
                middle1,
                middle2,
                target
            };

            var bezier = new BezierPath(points);

            var path = new VertexPath(bezier, projectilesContainer);

            return path;
        }

        private void OnProjectileDeath(DefaultGunProjectile projectile)
        {
            _projectilePool.Release(projectile);
        }
    }
}
