using NodeCanvas.DialogueTrees;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeCanvas.Editor
{
    [CustomEditor(typeof(DialogueActor))]
    public class DialogueActorEditor: UnityEditor.Editor
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
                preview.style.backgroundImage = sprite != null
                    ? new StyleBackground(sprite)
                    : new StyleBackground(StyleKeyword.Initial);
            });
            return root;
        }
    }
}
