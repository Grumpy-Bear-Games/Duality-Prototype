using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        private readonly Dictionary<string, Dictionary<string, object>> _state = new();

        public void Save() => CaptureState(_state);
        public void Load() => RestoreState(_state);
        public void Clear() => _state.Clear();
        
        public static void CaptureState(Dictionary<string, Dictionary<string, object>> state)
        {
            foreach (var saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                state[saveableEntity.ID] = saveableEntity.CaptureState();
            }
        }

        public static void RestoreState(Dictionary<string, Dictionary<string, object>> state)
        {
            foreach (var saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                if (!state.ContainsKey(saveableEntity.ID)) continue;
                
                saveableEntity.RestoreState(state[saveableEntity.ID]);
            }
        }
    }
}
