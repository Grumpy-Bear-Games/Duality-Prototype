using DualityGame.Realm;
using Games.GrumpyBear.Core.Observables;
using UnityEngine;

namespace DualityGame.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private RealmManager _realmManager;

        public IReadonlyObservable<ItemType> CurrentItem => _currentItem;
        private readonly Observable<ItemType> _currentItem = new();

        public void PickupItem(Item item)
        {
            DropItem();
            _currentItem.Set(item.Type);
            Destroy(item.gameObject);
        }

        public void DropItem()
        {
            if (_currentItem.Value == null) return;
            _currentItem.Value.Spawn(_realmManager.CurrentRealm.Value, transform.position);
            _currentItem.Set(null);
        }
    }
}
