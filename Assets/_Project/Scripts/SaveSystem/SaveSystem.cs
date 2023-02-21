using System.Collections.Generic;
using Games.GrumpyBear.Core.SaveSystem;

namespace DualityGame.SaveSystem
{
    using EntityStates = Dictionary<string, Dictionary<string, object>>;
    
    public class SaveSystem
    {
        private const string SaveFileName = "Game";

        public static void Save()
        {
            var state = FileSystem.LoadFile<EntityStates>(SaveFileName);
            SaveableEntity.CaptureEntityStates(state);
            FileSystem.SaveFile(SaveFileName, state);
        }

        public static void Load()
        {
            var state = FileSystem.LoadFile<EntityStates>(SaveFileName);
            SaveableEntity.RestoreEntityStates(state);
        }

        public static bool SavefileExists => FileSystem.Exists(SaveFileName);

        public static void Clear() => FileSystem.Delete(SaveFileName);
    }
}
