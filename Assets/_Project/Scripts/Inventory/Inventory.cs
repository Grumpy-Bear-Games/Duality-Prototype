using DualityGame.Realm;
using Games.GrumpyBear.Core.Observables;
using UnityEngine;

namespace DualityGame.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private RealmObservable _realm;
        
        public IReadonlyObservable<Item> CurrentItem => _currentItem;
        private readonly Observable<Item> _currentItem = new();

        public void PickupItem(Item item)
        {
            DropItem();
            _currentItem.Set(item);
            item.gameObject.SetActive(false);
        }

        public Item TakeFromInventory()
        {
            if (_currentItem.Value == null) return null;
            var item = _currentItem.Value;
            _currentItem.Set(null);
            return item;
        }

        public void DropItem()
        {
            if (_currentItem.Value == null) return;

            var droppedItem = _currentItem.Value; 
            droppedItem.transform.position = transform.position;
            droppedItem.gameObject.layer = _realm.Value.LevelLayer;
            droppedItem.gameObject.SetActive(true);
            
            _currentItem.Set(null);
        }
    }
}
