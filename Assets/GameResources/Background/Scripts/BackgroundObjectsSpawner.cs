using UnityEngine;

namespace GameResources.Background.Scripts
{
    using System.Collections.Generic;
    using UnityEngine.Serialization;

    public class BackgroundObjectsSpawner : MonoBehaviour
    {
        [SerializeField]
        private BackgroundObject backgroundObject;

        [SerializeField]
        private Transform startPoint;
        
        [SerializeField]
        private Transform endPoint;

        [SerializeField]
        private float speed;
        
        [SerializeField]
        private float distanceBetweenObjects = 2;

        [SerializeField]
        private List<BackgroundObject> objects = new List<BackgroundObject>();

        private void Update()
        {
            Move();

            var lastObjectDistance = float.MaxValue;

            if (objects.Count > 0)
            {
                lastObjectDistance = startPoint.position.x - objects[^1].transform.position.x;
            }

            if (lastObjectDistance > distanceBetweenObjects)
            {
                Spawn();
            }

            var firstObjectDistance = 1f;

            if (objects.Count > 0) 
            {
                firstObjectDistance = objects[0].transform.position.x - endPoint.position.x;
            }
            
            if (firstObjectDistance <= 0)
            {
                Despawn();
            }
        }

        private void Move()
        {
            foreach (var obj in objects)
            {
                obj.transform.position = Vector3.MoveTowards
                (
                    obj.transform.position, 
                    endPoint.position, 
                    speed * Time.fixedDeltaTime * Time.timeScale
                );
            }
        }
        
        private void Spawn()
        {
            var obj = Instantiate
            (
                backgroundObject,
                startPoint.position,
                Quaternion.Euler(0, Random.Range(-180, 180), 0),
                transform
            );
            
            objects.Add(obj);
        }

        private void Despawn()
        {
            var obj = objects[0];

            objects.RemoveAt(0);

            Destroy(obj.gameObject);
        }
    }
}
