using UnityEngine;
using UnityEngine.UI;

namespace DualityGame.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Image _image;

        private void OnEnable() => _inventory.CurrentItem.Subscribe(OnInventoryChange);
        private void OnDisable() => _inventory.CurrentItem.Unsubscribe(OnInventoryChange);

        private void OnInventoryChange(Item item)
        {
            _image.sprite = item != null ? item.Type.InventorySprite : null;
            _image.enabled = item != null;
        }
    }
}
