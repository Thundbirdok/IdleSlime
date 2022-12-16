using UnityEngine;

namespace GameResources.Movement
{
    using System;
    using PathCreation;

    [Serializable]
    public class PathFollower
    {
        public event Action OnTarget;
        
        [NonSerialized]
        public bool IsMoving;
        
        [SerializeField]
        private float speed = 12f;

        [SerializeField]
        private bool isSimulateGravity;

        private const float EPSILON = 0.001f;
        
        private Transform _transform;
        private VertexPath _vertexPath;

        private float _distance;

        private float _time;
        
        private Vector3 _previousPosition;
        
        private void OnDisable() => IsMoving = false;

        public void Init(Transform transform, VertexPath vertexPath)
        {
            _transform = transform;
            _vertexPath = vertexPath;

            _distance = 0;
            _time = 0;
            
            _previousPosition = _transform.position;
            
            IsMoving = true;
        }

        
        
        public void Move()
        {
            if (IsMoving == false)
            {
                return;
            }

            //SetDistance();

            _time += Time.fixedDeltaTime;
            
            _previousPosition = _transform.position;
            
            if (_time >= 1)
            {
                _transform.position = _vertexPath.GetPointAtTime(1);
                
                OnTarget?.Invoke();
                
                return;
            }
            
            _transform.position = _vertexPath.GetPointAtTime(_time);
        }

        private void SetDistance()
        {
            _distance += speed * Time.fixedDeltaTime * GetMultiplier();
        }

        private float GetMultiplier()
        {
            if (isSimulateGravity == false)
            {
                return 1;
            }

            var direction = _transform.position - _previousPosition;

            float multiplier;

            if (direction.magnitude <= EPSILON)
            {
                multiplier = 1;
            }
            else
            {
                var flat = direction;
                flat.y = 0;

                multiplier = direction.magnitude / flat.magnitude;
            }

            return multiplier;
        }
    }
}
