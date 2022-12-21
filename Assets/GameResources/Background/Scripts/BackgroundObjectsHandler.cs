namespace GameResources.Background.Scripts
{
    using System;
    using System.Collections.Generic;
    using Object = UnityEngine.Object;

    public class BackgroundObjectsHandler
    {
        public int Count => _objects?.Count ?? 0;

        [NonSerialized]
        private readonly List<BackgroundObject> _objects = new List<BackgroundObject>();
        public IReadOnlyList<BackgroundObject> Objects => _objects;

        public void Add(BackgroundObject backgroundObject)
        {
            _objects.Add(backgroundObject);
        }

        public void Remove(BackgroundObject backgroundObject)
        {
            _objects.Remove(backgroundObject);
        }

        public BackgroundObject First()
        {
            if (_objects == null || Count == 0)
            {
                return null;
            }
            
            return _objects[0];
        }

        public BackgroundObject Last()
        {
            if (_objects == null || Count == 0)
            {
                return null;
            }
            
            return _objects[^1];
        }

        public void Clear()
        {
            foreach (var obj in _objects)
            {
                Object.Destroy(obj.gameObject);
            }
            
            _objects.Clear();
        }
    }
}
