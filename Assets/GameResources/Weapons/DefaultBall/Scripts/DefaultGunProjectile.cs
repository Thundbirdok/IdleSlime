using UnityEngine;

namespace GameResources.Weapons.DefaultBall.Scripts
{
    using System;
    using GameResources.Movement;
    using PathCreation;

    public class DefaultGunProjectile : MonoBehaviour
    {
        public event Action<DefaultGunProjectile> OnDeath;
        
        [SerializeField]
        private PathFollower pathFollower;

        private void OnEnable()
        {
            pathFollower.OnTarget += Destroy;
        }

        private void OnDisable()
        {
            pathFollower.OnTarget -= Destroy;
            
            pathFollower.IsMoving = false;
        }

        private void FixedUpdate() => pathFollower.Move();

        private void OnCollisionEnter(Collision _) => Destroy();

        public void Fire(VertexPath vertexPath)
        {
            pathFollower.Init(transform, vertexPath);

            pathFollower.IsMoving = true;
        }
        
        private void Destroy() => OnDeath?.Invoke(this);
    }
}
