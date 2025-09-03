using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    public static class Converters
    {
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        #else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        #endif
        public static void RegisterConverters()
        {
            var spriteConverter = new ConverterGroup("Sprite");
            spriteConverter.AddConverter((ref Sprite sprite) => sprite is null ? new StyleBackground(StyleKeyword.Null) : new StyleBackground(sprite));
            ConverterGroups.RegisterConverterGroup(spriteConverter);
        }
    }
}
