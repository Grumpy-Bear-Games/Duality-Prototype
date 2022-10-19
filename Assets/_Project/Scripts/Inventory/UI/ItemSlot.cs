using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Inventory.UI
{
    public class ItemSlot
    {
        private readonly VisualElement _itemSprite;

        public ItemSlot(Button slotFrame, VisualElement itemSprite, Action onClicked)
        {
            _itemSprite = itemSprite;
            slotFrame.RegisterCallback<ClickEvent>(e => onClicked?.Invoke());
            SetSelected(false);
        }
        
        public void SetSelected(bool selected)
        {
            // Something visual should happen here
            Debug.Log(selected ? "Selecting" : "Deselecting");
        }

        public void SetItem(Item item)
        {
            if (item == null)
            {
                // No item
                Clear();
                return;
            }

            _itemSprite.style.backgroundImage = new StyleBackground(item.Type.InventorySprite);
            _itemSprite.RemoveFromClassList("Hidden");
        }

        public void Clear()
        {
            _itemSprite.AddToClassList("Hidden");
        }
    }
}
