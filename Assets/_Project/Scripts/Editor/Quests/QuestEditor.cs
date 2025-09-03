using DualityGame.Quests;
using NodeCanvas.DialogueTrees;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _editor;

        public override VisualElement CreateInspectorGUI()
        {
            var root = _editor.CloneTree();

            root.Q<PropertyField>("NPC").RegisterCallback<SerializedPropertyChangeEvent>(ev =>
            {
                if (ev.target is not VisualElement ve) return;
                UpdateNPCPortrait(ve.parent.Q<Image>("NPCPortrait"), ev.changedProperty);
            });
            UpdateNPCPortrait(root.Q<Image>("NPCPortrait"), serializedObject.FindProperty("_npc"));

            if (target is Quest quest)
            {
                var status = root.Q<DropdownField>("Status");
                status.index = (int) quest.Status;
                status.RegisterValueChangedCallback(evt =>
                {
                    if (evt.currentTarget is not DropdownField dropdown) return;
                    quest.Status = (Quest.QuestStatus) dropdown.index;
                });

            }

            return root;
        }


        private static void UpdateNPCPortrait(Image image, SerializedProperty property)
        {
            if (property.objectReferenceValue == null)
            {
                image.sprite = null;
                return;
            }

            if (property.objectReferenceValue is not ActorAsset npc) return;
            image.sprite = npc.PortraitByMood(Mood.Neutral);
        }
    }
}
