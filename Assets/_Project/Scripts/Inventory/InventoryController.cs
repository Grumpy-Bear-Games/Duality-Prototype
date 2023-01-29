using UnityEngine;

namespace DualityGame.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;

        public void PickupItem(Item item) => _inventory.AddItem(item);
    }
}
