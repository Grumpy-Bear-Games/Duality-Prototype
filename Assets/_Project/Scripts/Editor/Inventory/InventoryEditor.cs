using System.Collections.Generic;
using DualityGame.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Inventory
{
    [CustomEditor(typeof(DualityGame.Inventory.Inventory))]
    public class InventoryEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _runtimeInspectorTemplate;

        private DualityGame.Inventory.Inventory _inventory;
        private readonly List<ItemType> _cachedItemTypes = new();

        private void OnEnable()
        {
            _inventory = target as DualityGame.Inventory.Inventory;
            Debug.Assert(_inventory != null);

            _cachedItemTypes.Clear();
            foreach (var guid in AssetDatabase.FindAssets("t:ItemType"))
            {
                var itemType = AssetDatabase.LoadAssetAtPath<ItemType>(AssetDatabase.GUIDToAssetPath(guid));
                _cachedItemTypes.Add(itemType);
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement
            {
                dataSource = _inventory
            };
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                root.Add(CreateRuntimeInspector());
            }
            return root;
        }

        private VisualElement CreateRuntimeInspector()
        {
            var root = _runtimeInspectorTemplate.CloneTree();
            var listview = root.Q<ListView>();

            listview.overridingAddButtonBehavior = OverridingAddButtonBehavior;
            listview.onRemove += view =>
            {
                if (view.selectedItem is not ItemType itemType) return;
                _inventory.RemoveItem(itemType);
            };

            root.Q<Button>("ClearInventory").clicked += _inventory.Clear;
            return root;
        }

        private void OverridingAddButtonBehavior(BaseListView baseListView, Button addButton)
        {
            if (baseListView is not ListView listview) return;
            var menu = new GenericDropdownMenu();
            foreach (var itemType in _cachedItemTypes)
            {
                var menuItem = listview.itemTemplate.CloneTree();
                menuItem.dataSource = itemType;
                menuItem.RegisterCallback<MouseDownEvent>(_ => _inventory.AddItem(itemType));
                menu.AddItem(itemType.Name, menuItem);
            }
            menu.DropDown(addButton.worldBound, addButton);
        }
    }
}
