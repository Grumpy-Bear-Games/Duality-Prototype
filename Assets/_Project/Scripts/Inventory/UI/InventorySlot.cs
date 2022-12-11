using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Inventory.UI
{
    public class InventorySlot : VisualElement
    {
        private readonly VisualElement _itemSprite;

        public new class UxmlFactory : UxmlFactory<InventorySlot> { }

        private const string StyleResource = "InventorySlot";
        private const string USSClassNameBase = "inventory-slot";
        private const string FrameUssClassName = USSClassNameBase + "__frame";
        private const string ItemSpriteUssClassName = USSClassNameBase + "__item-sprite";
        private const string HiddenClass = "Hidden";

        public InventorySlot()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
            AddToClassList(USSClassNameBase);
            
            var frame = new VisualElement
            {
                name = "Frame",
                pickingMode = PickingMode.Ignore
            };
            frame.AddToClassList(FrameUssClassName);
            
            _itemSprite = new VisualElement
            {
                name = "ItemSprite",
                pickingMode = PickingMode.Ignore
            };
            _itemSprite.AddToClassList(ItemSpriteUssClassName);
            frame.Add(_itemSprite);
            hierarchy.Add(frame);
            pickingMode = PickingMode.Position;
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
                ClearItem();
                return;
            }

            _itemSprite.style.backgroundImage = new StyleBackground(item.Type.InventorySprite);
            _itemSprite.RemoveFromClassList(HiddenClass);
        }

        public void ClearItem() => _itemSprite.AddToClassList(HiddenClass);
    }
}
