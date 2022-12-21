using UnityEngine;

namespace GameResources.Background.Scripts
{
    public class BackgroundObject : MonoBehaviour
    {
        private BackgroundObjectModelsPool _backgroundObjectModelsPool;
        
        private GameObject _model;

        private int _index;
        
        public void Init(BackgroundObjectModelsPool backgroundObjectModelsPool)
        {
            _backgroundObjectModelsPool = backgroundObjectModelsPool;
        }

        public void SpawnModel()
        {
            _model = _backgroundObjectModelsPool.GetRandom(out _index);

            _model.transform.SetParent(transform);

            var rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
            
            _model.transform.SetLocalPositionAndRotation(Vector3.zero, rotation);
            
        }
        
        public void DespawnModel()
        {
            _backgroundObjectModelsPool.Release(_index, _model);
            _index = -1;
            _model = null;
        }
    }
}
