using System;
using DualityGame.Inventory;
using DualityGame.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomPropertyDrawer(typeof(Quest.QuestNPCVisibility))]
    public class QuestNPCVisibilityPropertyDrawer : PropertyDrawer
    {
        [SerializeField] private VisualTreeAsset _editor;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = _editor.CloneTree();
            root.Q<PropertyField>("Visibility").RegisterCallback<SerializedPropertyChangeEvent>(ev =>
            {
                if (ev.target is not VisualElement ve) return;
                UpdateCheckpointsVisibility(ve.parent.Q<VisualElement>("Checkpoints"), ev.changedProperty);
            });

            var checkpoints = root.Q<VisualElement>("Checkpoints");
            var noCheckpointWarning = checkpoints.Q<HelpBox>("EmptyCheckPointsWarning");
            var checkpointsListview = checkpoints.Q<ListView>();
            var checkpointsProperty = property.FindPropertyRelative("_checkpoints");
            Action updateCallback = () =>
            {
                noCheckpointWarning.style.display = checkpointsProperty.arraySize == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            };
            checkpointsListview.itemsSourceChanged += () => checkpoints.schedule.Execute(updateCallback);
            checkpointsListview.itemsAdded += _ => checkpoints.schedule.Execute(updateCallback);
            checkpointsListview.itemsRemoved += _ => checkpoints.schedule.Execute(updateCallback);

            UpdateCheckpointsVisibility(root.Q<VisualElement>("Checkpoints"), property.FindPropertyRelative("_visibility"));
            return root;
        }

        private static void UpdateCheckpointsVisibility(VisualElement ve, SerializedProperty property)
        {
            var value = (Quest.NPCVisibilityCondition) property.enumValueIndex;
            ve.style.display = value switch
            {
                Quest.NPCVisibilityCondition.AlwaysVisible => DisplayStyle.None,
                Quest.NPCVisibilityCondition.VisibleAfterCheckpoints => DisplayStyle.Flex,
                Quest.NPCVisibilityCondition.VisibleAfterComplete => DisplayStyle.None,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
