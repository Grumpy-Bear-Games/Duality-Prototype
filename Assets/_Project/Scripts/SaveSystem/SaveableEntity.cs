using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.SaveSystem
{
    public class SaveableEntity : MonoBehaviour
    {
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
                if (!state.ContainsValue(componentID)) continue;
                saveableComponent.RestoreState(state[componentID]);
            }
        }

        private static string GetComponentID(ISaveableComponent saveableComponent) =>
            saveableComponent.GetType().ToString();
    }
}
