using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DualityGame.SaveSystem
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [Tooltip("The unique ID is automatically generated in a scene file if " +
                 "left empty. Do not set in a prefab unless you want all instances to " + 
                 "be linked.")]        
        [SerializeField] private string _id;

        public string ID => _id;

        public Dictionary<string, object> CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                state[GetComponentID(saveableComponent)] = saveableComponent.CaptureState();
            }
            return state;
        }

        public void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                var componentID = GetComponentID(saveableComponent);
                if (!state.ContainsKey(componentID)) continue;
                saveableComponent.RestoreState(state[componentID]);
            }
        }

        private static string GetComponentID(ISaveableComponent saveableComponent) =>
            saveableComponent.GetType().ToString();
        
#if UNITY_EDITOR
        private static readonly Dictionary<string, SaveableEntity> globalLookup = new();
        
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("_id");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate]._id == candidate) return false;
            globalLookup.Remove(candidate);
            return true;

        }
#endif
    }
}
