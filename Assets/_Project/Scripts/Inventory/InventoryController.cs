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
            DropItem();
            _inventory.Set(item);
            item.gameObject.SetActive(false);
        }

        public void DropItem()
        {
            if (_inventory.Value == null) return;

            var droppedItem = _inventory.Value; 
            droppedItem.transform.position = transform.position;
            droppedItem.gameObject.layer = _realm.Value.LevelLayer;
            droppedItem.gameObject.SetActive(true);
            
            _inventory.Set(null);
        }
    }
}
