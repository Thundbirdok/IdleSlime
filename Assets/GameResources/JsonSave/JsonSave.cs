using UnityEngine;

namespace GameResources.JsonSave
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonSave
    {
        [NonSerialized]
        private JObject _jObject;

        private string jsonPath;

        private string _key;
        
        public JsonSave(string fileName, string key)
        {
            jsonPath = Application.persistentDataPath + fileName;
            _key = key;
        }
        
        public int Load()
        {
            GetJObject();

            return GetValue();
        }
        
        public void Save(int value)
        {
            var saveJObject = CollectResourcesToJObject(value);

            using var file = File.CreateText(jsonPath);
            using var writer = new JsonTextWriter(file);
            saveJObject.WriteTo(writer);
        }

        private void GetJObject()
        {
            try
            {
                using var file = File.OpenText(jsonPath);
                using var reader = new JsonTextReader(file);

                _jObject = (JObject)JToken.ReadFrom(reader);
            }
            catch (FileNotFoundException)
            {
                _jObject = new JObject();
            }
        }

        private int GetValue()
        {
            var token = _jObject.SelectToken(_key);

            return token?.Value<int>() ?? 0;
        }

        private JObject CollectResourcesToJObject(int value)
        {
            var saveJObject = new JObject();

            saveJObject.Add(_key, value);

            return saveJObject;
        }
    }
}
