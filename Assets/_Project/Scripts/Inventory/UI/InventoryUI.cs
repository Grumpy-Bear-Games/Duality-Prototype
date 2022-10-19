using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace DualityGame.Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InputActionReference _input;

        private UIDocument _uiDocument;
        
        private readonly List<ItemSlot> _slots = new();

        private int _currentPageIndex = 0;
        private int _currentSlotSelected = 0;
        private Button _previousPage;
        private Button _nextPage;
        private Label _itemTypeLabel;
        private int MaxPageIndex => (_inventory.Items.Count - 1)  / _slots.Count;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _input.action.Enable();
            _input.action.performed += OnInventoryToggle;
            
            _previousPage = _uiDocument.rootVisualElement.Q<Button>("PreviousPage");
            _nextPage = _uiDocument.rootVisualElement.Q<Button>("NextPage");
            _itemTypeLabel = _uiDocument.rootVisualElement.Q<Label>("ItemType");
            
            _previousPage.RegisterCallback<ClickEvent>(e => PreviousPage());
            _nextPage.RegisterCallback<ClickEvent>(e => NextPage());

            var index = 0;
            _uiDocument.rootVisualElement.Query<Button>(null, "ItemSlot").ForEach(
                slot =>
                {
                    var sprite = slot.Q<VisualElement>(null, "ItemSprite");
                    var itemSlot = new ItemSlot(slot, sprite, () => SelectSlot(index++));
                    _slots.Add(itemSlot);
                }
            );
            _slots[0].SetSelected(true);
        }

        private void SelectSlot(int index)
        {
            if (index != _currentSlotSelected)
            {
                _slots[_currentSlotSelected].SetSelected(false);
                _currentSlotSelected = index;
                _slots[_currentSlotSelected].SetSelected(true);
            }

            UpdateSelectedSlotUI();
        }

        private void UpdateSelectedSlotUI()
        {
            var itemIndex = (_currentPageIndex * _slots.Count) + _currentSlotSelected;

            if (itemIndex >= _inventory.Items.Count)
            {
                _itemTypeLabel.text = "";
                Debug.Log($"Selecting slot {_currentSlotSelected} (empty)");
            }
            else
            {
                var item = _inventory.Items[itemIndex];
                _itemTypeLabel.text = item.name;
                Debug.Log($"Selecting slot {_currentSlotSelected} ({item.name})");
            }
        }

        private void OnInventoryToggle(InputAction.CallbackContext obj) => _uiDocument.rootVisualElement.ToggleInClassList("Hidden");

        private void OnDestroy()
        {
            _input.action.Disable();
            _input.action.performed -= OnInventoryToggle;
        }

        private void OnEnable()
        {
            _inventory.OnChange += OnInventoryChange;
            OnInventoryChange();
        }

        private void NextPage()
        {
            if (_currentPageIndex >= MaxPageIndex) return;
            _currentPageIndex++;
            SelectSlot(0);
            RedrawInventorySlots();
            UpdatePageButtons();
        }

        private void PreviousPage()
        {
            if (_currentPageIndex <= 0) return;
            _currentPageIndex--;
            SelectSlot(0);
            RedrawInventorySlots();
            UpdatePageButtons();
        }

        public void OnDisable()
        {
            _inventory.OnChange -= OnInventoryChange;
        }

        private void RedrawInventorySlots()
        {
            for (var slotIndex = 0; slotIndex < _slots.Count; slotIndex++)
            {
                var itemIndex = (_currentPageIndex * _slots.Count) + slotIndex;
                if (itemIndex >= _inventory.Items.Count) {
                    _slots[slotIndex].Clear();
                } else {
                    _slots[slotIndex].SetItem(_inventory.Items[itemIndex]);
                }
            }
        }

        private void UpdatePageButtons()
        {
            _previousPage.visible = _currentPageIndex > 0;
            _nextPage.visible = _currentPageIndex < MaxPageIndex;
        }


        private void OnInventoryChange()
        {
            _currentPageIndex = Math.Clamp(_currentPageIndex, 0, MaxPageIndex);
            RedrawInventorySlots();
            UpdatePageButtons();
            UpdateSelectedSlotUI();
        }
    }
}
