using NodeCanvas.DialogueTrees;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeCanvas.Editor
{
    [CustomEditor(typeof(ActorAsset))]
    public class ActorAssetEditor: UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _editor;
        [SerializeField] private VisualTreeAsset _portraitItem;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = _editor.CloneTree();
            var portraitsListView = root.Q<ListView>();
            portraitsListView.makeItem = MakeItem;
            return root;
        }

        private VisualElement MakeItem()
        {
            var root = _portraitItem.CloneTree();

            var portraitPropertyField = root.Q<PropertyField>("Portrait");
            var preview = root.Q<VisualElement>("SpritePreview");
            portraitPropertyField.RegisterValueChangeCallback(e =>
            {
                var sprite = e.changedProperty.objectReferenceValue as Sprite;
                preview.style.backgroundImage = new StyleBackground(sprite);
                preview.style.display = sprite != null ? DisplayStyle.Flex : DisplayStyle.None;
            });
            return root;
        }
    }
}
