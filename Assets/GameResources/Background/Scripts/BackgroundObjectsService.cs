using UnityEngine;

namespace GameResources.Background.Scripts
{
    public class BackgroundObjectsService : MonoBehaviour
    {
        [SerializeField]
        private BackgroundObjectsSpawner spawner;

        [SerializeField]
        private Transform startPoint;
        
        [SerializeField]
        private Transform endPoint;

        [SerializeField]
        private float speed = 2.5f;
        
        [SerializeField]
        private float distanceBetweenObjects = 2;
        
        private BackgroundObjectsHandler _objectsHandler;

        private void Awake()
        {
            _objectsHandler = new BackgroundObjectsHandler();
            
            spawner.Construct
            (
                _objectsHandler,
                startPoint.position,
                endPoint.position,
                distanceBetweenObjects
            );
        }

        private void OnEnable()
        {
            if (spawner.IsInited)
            {
                spawner.FirstPopulate();
                
                return;
            }

            spawner.OnInited += spawner.FirstPopulate;
        }

        private void OnDisable()
        {
            spawner.OnInited -= spawner.FirstPopulate;
        }

        private void OnDestroy() => spawner.Dispose();

        private void Update()
        {
            Move();

            SpawnObjects();
        }

        private void SpawnObjects()
        {
            if (IsNeedSpawn())
            {
                spawner.Spawn();
            }

            if (IsNeedDespawn())
            {
                spawner.Despawn();
            }
        }

        private bool IsNeedSpawn()
        {
            var last = _objectsHandler.Last();

            if (last == null)
            {
                return true;
            }

            var isClose = IsCloseTo
            (
                last.transform.position,
                startPoint.position,
                distanceBetweenObjects
            );
            
            return isClose == false;
        }

        private bool IsNeedDespawn()
        {
            var first = _objectsHandler.First();

            if (first == null)
            {
                return true;
            }

            var isClose = IsCloseTo
            (
                endPoint.position,
                first.transform.position,
                0
            );
            
            return isClose;
        }
        
        private bool IsCloseTo(Vector3 left, Vector3 right, float triggerDistance)
        {
            var distance = right.x - left.x;

            return distance <= triggerDistance;
        }
        
        private void Move()
        {
            foreach (var obj in _objectsHandler.Objects)
            {
                obj.transform.position = Vector3.MoveTowards
                (
                    obj.transform.position, 
                    endPoint.position, 
                    speed * Time.fixedDeltaTime * Time.timeScale
                );
            }
        }
    }
}
