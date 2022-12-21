using UnityEngine;

namespace GameResources.Background.Scripts
{
    using System;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;

    [Serializable]
    public class BackgroundObjectsSpawner
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }
        
        [SerializeField]
        private BackgroundObjectModelsPool backgroundObjectModelsPool;
        
        [SerializeField]
        private BackgroundObject backgroundObjectPrefab;

        [SerializeField]
        private Transform objectsContainer;

        [NonSerialized]
        private BackgroundObjectsHandler _objectsHandler;
        
        private ObjectPool<BackgroundObject> _pool;

        private Vector3 _startPoint;

        private Vector3 _endPoint;

        private float _distanceBetweenObjects = 2;

        public void Construct(BackgroundObjectsHandler objectHandler, Vector3 startPoint, Vector3 endPoint, float distanceBetweenObjects)
        {
            _objectsHandler = objectHandler;
            
            _startPoint = startPoint;
            _endPoint = endPoint;

            _distanceBetweenObjects = distanceBetweenObjects;
            
            backgroundObjectModelsPool.Init();
            
            InitPool();

            IsInited = true;
            OnInited?.Invoke();
        }

        public void Dispose()
        {
            DestroyObjects();

            IsInited = false;
        }

        public void Spawn() => _pool.Get();

        public void Despawn() => _pool.Release(_objectsHandler.First());

        public void FirstPopulate()
        {
            var currentPoint = _endPoint;
            
            while (currentPoint.x - _startPoint.x < 0)
            {
                var obj = _pool.Get();

                obj.transform.position = currentPoint;

                currentPoint.x += _distanceBetweenObjects;
            }
        }

        private void InitPool()
        {
            var numberOfPoints =
                Mathf.FloorToInt
                (
                    (_startPoint.x - _endPoint.x) / _distanceBetweenObjects
                );
            
            _pool = new ObjectPool<BackgroundObject>
            (
                CreateFunc,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false,
                numberOfPoints,
                numberOfPoints + 2
            );
        }

        private void DestroyObjects()
        {
            _pool.Clear();
            
            _pool = null;

            _objectsHandler.Clear();
        }

        private BackgroundObject CreateFunc()
        {
            var obj = Object.Instantiate
            (
                backgroundObjectPrefab,
                objectsContainer
            );
            
            obj.Init(backgroundObjectModelsPool);

            return obj;
        }

        private void ActionOnGet(BackgroundObject obj)
        {
            _objectsHandler.Add(obj);
            
            obj.SpawnModel();

            obj.transform.position = _startPoint;

            obj.gameObject.SetActive(true);
        }

        private void ActionOnRelease(BackgroundObject obj)
        {
            _objectsHandler.Remove(obj);

            obj.DespawnModel();
            
            obj.gameObject.SetActive(false);
        }

        private static void ActionOnDestroy(BackgroundObject obj) => Object.Destroy(obj.gameObject);
    }
}
