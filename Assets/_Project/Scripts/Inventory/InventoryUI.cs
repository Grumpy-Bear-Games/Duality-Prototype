using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DualityGame.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Image _image;

        private void OnEnable()
        {
            _inventory.OnChange += OnInventoryChange;
            OnInventoryChange();
        }

        private void OnDisable() => _inventory.OnChange -= OnInventoryChange;

        private void OnInventoryChange()
        {
            var item = _inventory.Items.Any() ? _inventory.Items[0] : null;
            _image.sprite = item != null ? item.Type.InventorySprite : null;
            _image.enabled = item != null;
        }
    }
}
