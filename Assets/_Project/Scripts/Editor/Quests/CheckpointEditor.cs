using DualityGame.Quests;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomEditor(typeof(Checkpoint))]
    public class CheckpointEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _editor;

        public override VisualElement CreateInspectorGUI()
        {
            var root = _editor.CloneTree();
            root.dataSource = this.target as Checkpoint;
            return root;
        }
    }
}
