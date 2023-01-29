using UnityEngine;

namespace DualityGame.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;

        public void PickupItem(ItemType item) => _inventory.AddItem(item);
    }
}
