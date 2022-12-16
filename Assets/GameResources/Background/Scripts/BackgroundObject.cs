using UnityEngine;

namespace GameResources.Background.Scripts
{
    public class BackgroundObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] models;

        private GameObject _model;
        
        private void OnEnable()
        {
            Spawn();
        }

        private void OnDisable()
        {
            Despawn();
        }

        private void Spawn()
        {
            _model = Instantiate(models[Random.Range(0, models.Length)], transform);
        }

        private void Despawn()
        {
            if (_model == null)
            {
                return;
            }
            
            Destroy(_model);

            _model = null;
        }
    }
}
