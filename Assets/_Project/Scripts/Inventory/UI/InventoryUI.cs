using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DualityGame.Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InputActionReference _input;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private GameObject _inventoryFrame;
        
        private readonly List<ItemSlot> _slots = new();

        private int _currentPageIndex = 0;
        private int _currentSlotSelected = 0;
        private int MaxPageIndex => (_inventory.Items.Count - 1)  / _slots.Count;
        
        private void Awake()
        {
            _input.action.Enable();
            _slots.AddRange(GetComponentsInChildren<ItemSlot>());
            _input.action.performed += OnInventoryToggle;
        }

        private void Start()
        {
            for (var slotIndex = 0; slotIndex < _slots.Count; slotIndex++)
            {
                var index = slotIndex;
                _slots[slotIndex].OnClicked += () => SelectSlot(index);
                _slots[slotIndex].SetSelected(false);
            }
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
                Debug.Log($"Selecting slot {_currentSlotSelected} (empty)");
            }
            else
            {
                var item = _inventory.Items[itemIndex];
                Debug.Log($"Selecting slot {_currentSlotSelected} ({item.name})");
            }
        }

        // TODO: Probably not how we want to hide and show the inventory UI later
        private void OnInventoryToggle(InputAction.CallbackContext obj) => _inventoryFrame.SetActive(!_inventoryFrame.activeSelf);

        private void OnDestroy()
        {
            _input.action.Disable();
            _input.action.performed -= OnInventoryToggle;
        }

        private void OnEnable()
        {
            _inventory.OnChange += OnInventoryChange;
            OnInventoryChange();
            
            _previousButton.onClick.AddListener(PreviousPage);
            _nextButton.onClick.AddListener(NextPage);
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
            _previousButton.onClick.RemoveListener(PreviousPage);
            _nextButton.onClick.RemoveListener(NextPage);
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
            _previousButton.gameObject.SetActive(_currentPageIndex > 0);
            _nextButton.gameObject.SetActive(_currentPageIndex < MaxPageIndex);
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
