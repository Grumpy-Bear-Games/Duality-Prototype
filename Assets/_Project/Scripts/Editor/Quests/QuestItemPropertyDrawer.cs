using System;
using DualityGame.Inventory;
using DualityGame.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomPropertyDrawer(typeof(Quest.QuestItem))]
    public class QuestItemPropertyDrawer : PropertyDrawer
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

            root.Q<PropertyField>("ItemType").RegisterCallback<SerializedPropertyChangeEvent>(ev =>
            {
                if (ev.target is not VisualElement ve) return;
                UpdateItemTypeSprite(ve.parent.Q<Image>("ItemTypeSprite"), ev.changedProperty);
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
            UpdateItemTypeSprite(root.Q<Image>("ItemTypeSprite"), property.FindPropertyRelative("_itemType"));
            return root;
        }

        private static void UpdateCheckpointsVisibility(VisualElement ve, SerializedProperty property)
        {
            var value = (Quest.QuestItemVisibility) property.enumValueIndex;
            ve.style.display = value switch
            {
                Quest.QuestItemVisibility.AlwaysVisible => DisplayStyle.None,
                Quest.QuestItemVisibility.VisibleAfterCheckpoints => DisplayStyle.Flex,
                Quest.QuestItemVisibility.VisibleWhenAcquired => DisplayStyle.None,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static void UpdateItemTypeSprite(Image image, SerializedProperty property)
        {
            if (property.objectReferenceValue == null)
            {
                image.sprite = null;
                return;
            }

            if (property.objectReferenceValue is not ItemType itemType) return;
            image.sprite = itemType.InventorySprite;
        }
    }
}
