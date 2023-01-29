namespace DualityGame.SaveSystem
{
    public class SaveSystem
    {
        private const string SaveFileName = "Game";

        public static void Save()
        {
            var state = FileSystem.LoadFile(SaveFileName);
            SaveableEntity.CaptureEntityStates(state);
            FileSystem.SaveFile(SaveFileName, state);
        }

        public static void Load()
        {
            var state = FileSystem.LoadFile(SaveFileName);
            SaveableEntity.RestoreEntityStates(state);
        }

        public static void Clear() => FileSystem.Delete(SaveFileName);
    }
}
