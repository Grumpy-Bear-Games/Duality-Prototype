using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.SaveSystem
{
    public class SaveSystem
    {
        private const string SaveFileName = "Game";

        public static void Save()
        {
            var state = FileSystem.LoadFile(SaveFileName);
            CaptureState(state);
            FileSystem.SaveFile(SaveFileName, state);
        }

        public static void Load()
        {
            var state = FileSystem.LoadFile(SaveFileName);
            RestoreState(state);
        }

        public static void Clear() => FileSystem.Delete(SaveFileName);

        public static void CaptureState(Dictionary<string, Dictionary<string, object>> state)
        {
            foreach (var saveableEntity in Object.FindObjectsOfType<SaveableEntity>())
            {
                state[saveableEntity.ID] = saveableEntity.CaptureState();
            }
        }

        public static void RestoreState(Dictionary<string, Dictionary<string, object>> state)
        {
            foreach (var saveableEntity in Object.FindObjectsOfType<SaveableEntity>())
            {
                if (!state.ContainsKey(saveableEntity.ID)) continue;
                
                saveableEntity.RestoreState(state[saveableEntity.ID]);
            }
        }
    }
}
