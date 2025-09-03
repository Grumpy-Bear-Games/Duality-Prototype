using DualityGame.Quests;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomPropertyDrawer(typeof(Quest.CheckpointGroup))]
    public class CheckpointGroupPropertyDrawer : PropertyDrawer
    {
        [SerializeField] private VisualTreeAsset _editor;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = _editor.CloneTree();
            return root;
        }
    }
}