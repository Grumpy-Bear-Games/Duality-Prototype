using DualityGame.Quests;
using UnityEditor;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    public static class QuestConverters {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void RegisterConverters()
        {
            var questItemVisibilityConverter = new ConverterGroup("QuestItemVisibility");
            questItemVisibilityConverter.AddConverter((ref Quest.QuestItemVisibility visibility) =>
                visibility == Quest.QuestItemVisibility.VisibleAfterCheckpoints
                    ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex)
                    : new StyleEnum<DisplayStyle>(DisplayStyle.None));
            ConverterGroups.RegisterConverterGroup(questItemVisibilityConverter);
        }
    }
}