using DualityGame.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Inventory
{
    [CustomEditor(typeof(ItemType))]
    public class ItemTypeEditor: UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _editor;
        private VisualElement _preview;

        public override VisualElement CreateInspectorGUI()
        {
            var root = _editor.CloneTree();
            root.Q<Label>("Name").text = target.name;

            _preview = root.Q<VisualElement>("InventorySpritePreview");

            root.Q<PropertyField>("InventorySprite")
                .RegisterCallback<ChangeEvent<Object>>(evt => UpdatePreview(evt.newValue as Sprite));
            if (target is ItemType itemType) UpdatePreview(itemType.InventorySprite);
            return root;
        }

        private void UpdatePreview(Sprite sprite)
        {
            _preview.style.backgroundImage = new StyleBackground(sprite);
            _preview.style.display = sprite != null ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
