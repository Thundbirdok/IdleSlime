using UnityEngine;

namespace GameResources.JsonSave
{
    using System.Collections.Generic;

    public class Saver : MonoBehaviour
    {
        [SerializeReference]
        private List<SavableScriptableObject> savables;

        private void Awake()
        {
            Load();
        }

        private void OnDestroy()
        {
            Save();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                return;
            }
            
            Save();
        }

        private void Load()
        {
            foreach (var savable in savables)
            {
                savable.Load();
            }
        }

        private void Save()
        {
            foreach (var savable in savables)
            {
                savable.Save();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Save();
            }
        }
    }
}
