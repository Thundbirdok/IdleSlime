using UnityEngine;

namespace GameResources.Weapons.DefaultBall.Scripts
{
    using System.Collections;
    using PathCreation;
    using UnityEngine.Pool;

    public class DefaultBallGun : MonoBehaviour
    {
        [SerializeField]
        private Collider ownerCollider;
        
        [SerializeField]
        private float fireRate = 12;

        [SerializeField] 
        private DefaultGunProjectile projectilePrefab;

        [SerializeField]
        private Transform projectileFirePoint;
        
        [SerializeField]
        private Transform projectilesContainer;

        [SerializeField]
        private Transform testTarget;
        
        private Coroutine _coroutine;

        private ObjectPool<DefaultGunProjectile> _pool;

        private void OnEnable()
        {
            _pool = new ObjectPool<DefaultGunProjectile>
            (
                CreateFunc, 
                ActionOnGet, 
                ActionOnRelease, 
                ActionOnDestroy, 
                false
            );
            
            _coroutine = StartCoroutine(AutoFire());
        }

        private static void ActionOnDestroy(DefaultGunProjectile projectile) => Destroy(projectile.gameObject);

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

        private void OnDisable()
        {
            if (_coroutine == null)
            {
                return;
            }

            StopCoroutine(_coroutine);

            _coroutine = null;
            
            _pool.Clear();
        }

        private IEnumerator AutoFire()
        {
            var delay = new WaitForSeconds(60 / fireRate);
            
            while (enabled)
            {
                Fire();
                
                yield return delay;
            }
        }

        private void Fire()
        {
            var projectile = _pool.Get();
            
            var path = GetProjectilePath(testTarget.position);

            projectile.Fire(path);
        }

        private VertexPath GetProjectilePath(Vector3 target)
        {
            var startPosition = projectileFirePoint.position;
            var direction = target - startPosition;

            var middle1 = startPosition + (direction / 3);
            var middle2 = startPosition + (direction / 3 * 2);

            var verticalDistance = target.y - startPosition.y;
            var middleHeight = Mathf.Max(verticalDistance, startPosition.y) + direction.magnitude * 0.1f;
            
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

        private void OnProjectileDeath(DefaultGunProjectile projectile) => _pool.Release(projectile);
    }
}
