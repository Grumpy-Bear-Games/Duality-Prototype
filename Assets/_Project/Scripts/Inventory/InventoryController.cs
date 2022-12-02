using DualityGame.Realm;
using UnityEngine;

namespace DualityGame.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private RealmObservable _realm;
        [SerializeField] private Inventory _inventory;

        public void PickupItem(Item item)
        {
            _inventory.AddItem(item);
            item.gameObject.SetActive(false);
        }
        
        public void DropInventory()
        {
            foreach (var item in _inventory.Items)
            {
                item.ReturnToInitialPosition();
            }
            _inventory.Clear();
        }
    }
}
