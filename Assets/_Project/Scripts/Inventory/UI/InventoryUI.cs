using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;

        private readonly List<InventorySlot> _slots = new();

        private int _currentPageIndex = 0;
        private int _currentSlotSelected = 0;
        private Button _previousPage;
        private Button _nextPage;
        private Label _itemTypeLabel;
        private VisualElement _frame;
        private int MaxPageIndex => (_inventory.Items.Count - 1)  / _slots.Count;

        #region Life-cycle management 
        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _frame = root.Q<VisualElement>("InventoryFrame");
            _previousPage = _frame.Q<Button>("PreviousPage");
            _nextPage = _frame.Q<Button>("NextPage");
            _itemTypeLabel = _frame.Q<Label>("ItemTypeName");
            
            _previousPage.clicked += PreviousPage;
            _nextPage.clicked += NextPage;

            _frame.Query<InventorySlot>().ForEach(_slots.Add);
            _slots[0].SetSelected(true);

            _frame.Q<VisualElement>("Slots").RegisterCallback<ClickEvent>(InventorySlotClicked);
        }

        private void OnEnable()
        {
            _inventory.OnChange += OnInventoryChange;
            OnInventoryChange();
            Show();
        }

        public void OnDisable()
        {
            Hide();
            _inventory.OnChange -= OnInventoryChange;
        }
        #endregion

        private void Hide() => _frame?.AddToClassList("Hidden");

        private void Show() => _frame?.RemoveFromClassList("Hidden");

        #region Inventory UI
        private void InventorySlotClicked(ClickEvent evt)
        {
            if (evt.target is not InventorySlot slot) return;
            SelectSlot(slot.parent.hierarchy.IndexOf(slot));
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

        private void RedrawInventorySlots()
        {
            for (var slotIndex = 0; slotIndex < _slots.Count; slotIndex++)
            {
                var itemIndex = (_currentPageIndex * _slots.Count) + slotIndex;
                if (itemIndex >= _inventory.Items.Count) {
                    _slots[slotIndex].ClearItem();
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
        #endregion
    }
}
