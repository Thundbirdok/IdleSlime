using UnityEngine;

namespace GameResources.Background.Scripts
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    [Serializable]
    public class BackgroundObjectModelsPool
    {
        [SerializeField]
        private Transform container;
        
        [SerializeField]
        private GameObject[] models;

        private List<BackgroundObjectModelPool> _pools;

        public void Init()
        {
            if (models == null || models.Length == 0)
            {
                Debug.Log("No models to spawn");
                
                return;
            }

            InitPool();
        }

        public GameObject GetRandom(out int index)
        {
            index = Random.Range(0, _pools.Count);
            
            return _pools[index].Get();
        }

        public void Release(int index, GameObject gameObject)
        {
            gameObject.transform.SetParent(container);
            
            _pools[index].Release(gameObject);
        }
        
        private void InitPool()
        {
            _pools = new List<BackgroundObjectModelPool>();

            for (var i = 0; i < models.Length; i++)
            {
                var pool = new BackgroundObjectModelPool (models[i], container);
                
                _pools.Add(pool);
            }
        }
    }

    public class BackgroundObjectModelPool
    {
        private readonly GameObject _model;
        
        private readonly Transform _container;
        
        private readonly ObjectPool<GameObject> _pool;

        public BackgroundObjectModelPool(GameObject model, Transform container)
        {
            _model = model;
            _container = container;
            
            _pool = new ObjectPool<GameObject>
            (
                CreateFunc,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
        }

        public GameObject Get()
        {
            return _pool.Get();
        }

        public void Release(GameObject gameObject)
        {
            _pool.Release(gameObject);
        }
        
        private GameObject CreateFunc()
        {
            return Object.Instantiate(_model, _container);
        }

        private static void ActionOnGet(GameObject model)
        {
            model.SetActive(true);
        }

        private static void ActionOnRelease(GameObject model)
        {
            model.SetActive(false);
        }

        private static void ActionOnDestroy(GameObject model)
        {
            Object.Destroy(model);
        }
    }
}
