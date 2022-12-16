namespace GameResources.JsonSave
{
    using UnityEngine;

    public abstract class SavableScriptableObject : ScriptableObject
    {
        public abstract void Load();
        public abstract void Save();
    }
}
