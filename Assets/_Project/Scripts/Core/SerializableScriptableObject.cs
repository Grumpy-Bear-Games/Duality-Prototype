#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.Core
{
    public abstract class SerializableScriptableObject<T>: ScriptableObject where T: SerializableScriptableObject<T>
    {
        [SerializeField] private string _guid;
        public string GUID => _guid;

        private static Dictionary<string, T> _instances = null;

        public static T GetByGUID(string guid)
        {
            if (_instances == null) FindAllInstances();
            _instances.TryGetValue(guid, out var instance);
            return instance;
        }

        private static void FindAllInstances()
        {
            _instances = new Dictionary<string, T>();
            foreach (var instance in Resources.FindObjectsOfTypeAll<T>())
            {
                _instances.Add(instance._guid, instance);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(path);
            AssetDatabase.SaveAssetIfDirty(this);
        }
        #endif
    }
}
